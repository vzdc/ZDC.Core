using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Facility
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }
        public bool Online { get; set; }
    }
}