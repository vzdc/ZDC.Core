using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class LastUpdated
    {
        [Key] public int Id { get; set; }
        public DateTime Time { get; set; }
    }
}