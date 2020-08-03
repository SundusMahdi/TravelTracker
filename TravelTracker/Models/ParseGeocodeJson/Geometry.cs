using TravelTracker.Models.ParseDestination;

namespace TravelTracker.Models
{
    public class Geometry
    {
        public Bounds bounds { get; set; }

        public LatLngJson location { get; set; }

        public string location_type { get; set; }

        public Bounds viewport { get; set; }
    }
}