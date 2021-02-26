using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class BaseModel
    {
        [Key] public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}