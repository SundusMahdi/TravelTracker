using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TravelTracker.Models
{
    public class DestinationCreateViewModel : Destination
    {
        [Display(Name = "Upload custom image (optional)")]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }
    }
}
