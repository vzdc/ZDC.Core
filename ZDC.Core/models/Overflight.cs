using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Overflight
    {
        [Key] public int Id { get; set; }

        public string Callsign { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Route { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}