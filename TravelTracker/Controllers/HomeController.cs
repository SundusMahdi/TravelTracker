using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelTracker.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Owin.BuilderProperties;
using System.Linq;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace TravelTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDestinationRepo _destinationRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        static readonly HttpClient client = new HttpClient();

        public HomeController(IDestinationRepo destinationRepo, IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _destinationRepo = destinationRepo;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
        }

        // Map View
        public IActionResult Map()
        {
            var result = _destinationRepo.Get(User.Identity.Name);
            result.ToList().ForEach(x => Console.WriteLine(x.Lat.ToString() + " " + x.Lng));
            return View(result);
        }

        // Index Route
        [HttpGet]
        public IActionResult Index()
        {
            var result =_destinationRepo.Get(User.Identity.Name);
            return View(result);
        }

        // Show Route
        [HttpGet]
        public IActionResult Show(int id)
        {
            var result = _destinationRepo.Get(id, User.Identity.Name);
            return View(result);
        }

        // New Route
        [HttpGet]
        public IActionResult New()
        {
            ViewBag.ApiKey = _config["ApiKey"];
            return View(new DestinationCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(DestinationCreateViewModel dest)
        {
            await GetStaticMapAsync(dest);
            return View(dest);
        }

        // Create Route
        [HttpPost]
        public IActionResult Create(DestinationCreateViewModel dest)
        {
            if (ModelState.IsValid)
            {
                // Sets the image with the following priorities:
                // 1. specified image in dest.Image
                // 2. static map in dest.ImageUrl
                string fileName;
                if (dest.Image != null)
                {
                    fileName = SaveImage(dest);
                }
                else
                {
                    Debug.Assert(dest.ImageUrl != null);
                    fileName = dest.ImageUrl;
                }

                Destination newDest = new Destination
                {
                    User = dest.User,
                    Address = dest.Address,
                    Lat = dest.Lat,
                    Lng = dest.Lng,
                    Details = dest.Details,
                    Visited = dest.Visited,
                    ImageFile = fileName
                };
                _destinationRepo.Add(newDest);
                return RedirectToAction("Show", new { id = newDest.Id });
            }
            return View("New",dest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DestinationCreateViewModel dest)
        {
            await GetStaticMapAsync(dest);
            return View(dest);
        }

        // Edit Route
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var destination = _destinationRepo.Get(id, User.Identity.Name);
            DestinationCreateViewModel dest = new DestinationCreateViewModel
            {
                Id = destination.Id,
                User = destination.User,
                Address = destination.Address,
                Lat = destination.Lat,
                Lng = destination.Lng,
                ImageFile = destination.ImageFile,
                Details = destination.Details,
                Visited = destination.Visited
            };
            ViewBag.ApiKey = _config["ApiKey"];
            return View(dest);
        }

        // Update Route
        [HttpPost]
        public IActionResult Update(DestinationCreateViewModel dest)
        {
            if (ModelState.IsValid)
            {
                string fileName = dest.ImageFile;
                // Delete old image
                DeleteImage(fileName);
                if (dest.Image != null )
                {
                    // Save new image
                    fileName = SaveImage(dest);
                }
                else if (dest.ImageUrl != null)
                {
                    // No image has been specified so save the static map as the image
                    fileName = dest.ImageUrl;
                }

                Destination updatedDest = _destinationRepo.Get(dest.Id, User.Identity.Name);
                Console.WriteLine("updatedDest" + updatedDest);
                Console.WriteLine(dest.Address);
                updatedDest.Address = dest.Address;
                updatedDest.Lat = dest.Lat;
                updatedDest.Lng = dest.Lng;
                updatedDest.Details = dest.Details;
                updatedDest.ImageFile = fileName;
                updatedDest.Visited = dest.Visited;

                _destinationRepo.Update(updatedDest);
                return RedirectToAction("Show", new { id = updatedDest.Id });
            }
            return View("Edit", dest);
        }

        // Destroy Route
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Destination deleteDest = _destinationRepo.Get(id, User.Identity.Name);
            _destinationRepo.Delete(id);
            DeleteImage(deleteDest.ImageFile);
            return RedirectToAction("Index", "Home");
        }

        private string SaveImage(DestinationCreateViewModel dest)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "destinations");
            string fileName = Guid.NewGuid() + "_" + dest.Image.FileName;
            filePath = Path.Combine(filePath, fileName);
            dest.Image.CopyTo(new FileStream(filePath, FileMode.Create));
            return fileName;
        }

        private void DeleteImage(string imageFile)
        {
            if (imageFile != "no-image.png" && !imageFile.StartsWith("https"))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "destinations", imageFile);
                System.IO.File.Delete(filePath);
            }
        }

        private async Task GetStaticMapAsync(DestinationCreateViewModel dest)
        {
            string address = dest.Address.Replace(' ', '+');
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=" + _config["ApiKey"]);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // For debugging purposes
                Console.WriteLine(responseBody);

                var result = JsonConvert.DeserializeObject<GeocodeJson>(responseBody);
                if (result.status == "OK")
                {
                    dest.Lat = result.results[0].geometry.location.lat;
                    dest.Lng = result.results[0].geometry.location.lng;
                    dest.Address = result.results[0].formatted_address;
                    Console.WriteLine(dest.Visited);
                    int zoom = 3;
                    if (result.results[0].geometry.bounds == null)
                    {
                        zoom = 19;
                    }
                    else
                    {
                        double size = Math.Max(Math.Abs(result.results[0].geometry.bounds.northeast.lat
                            - result.results[0].geometry.bounds.southwest.lat),
                            Math.Abs(result.results[0].geometry.bounds.northeast.lng
                            - result.results[0].geometry.bounds.southwest.lng));
                        zoom = (int)Math.Round(14 / (Math.Sqrt(size) + .9) + 2);
                    }
                    dest.ImageUrl = "https://maps.googleapis.com/maps/api/staticmap?size=768x512&zoom=" + zoom + "&center=" + dest.Lat + "," + dest.Lng + "&style=feature:landscape|element:all|hue:0xf5f5f5|lightness:50&style=feature:poi|element:all|visibility:off&style=feature:road|element:all|saturation:-100|lightness:45&style=feature:road.highway|element:all|visibility:simplified&style=feature:transit|element:all|visibility:off&style=feature:water|element:all|color:0x9acfd3|visibility:on&key=" + _config["ApiKey"];
                }
                else
                {
                    dest.Address = "";
                    ModelState.AddModelError(string.Empty, "No results found");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
