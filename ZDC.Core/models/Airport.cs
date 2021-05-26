using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Airport
    {
        public Airport()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string Icao { get; set; }
        public string Name { get; set; }
        public virtual Metar Metar { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}