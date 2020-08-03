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
    public class GeocodeJson
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}
