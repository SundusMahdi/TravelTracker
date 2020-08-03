using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelTracker.Models.ViewModels
{
    public class GetLatLngViewModel
    {
        [Required]
        public string Address { get; set; }
    }
}
