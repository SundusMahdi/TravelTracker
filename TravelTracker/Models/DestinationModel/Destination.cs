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
    public class Destination
    {
        [Required]
        public int Id { get; set; }

        public string User { get; set; }

        [Required]
        public string Address { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool Visited { get; set; }

        public string ImageFile { get; set; }

        public string Details {get; set;}
    }
}
