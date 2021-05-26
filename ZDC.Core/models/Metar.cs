using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Metar
    {
        public Metar()
        {
            Created = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string MetarRaw { get; set; }
        public string Conditions { get; set; }
        public string Wind { get; set; }
        public string Temp { get; set; }
        public string Altimeter { get; set; }
        public DateTime Created { get; set; }
    }
}