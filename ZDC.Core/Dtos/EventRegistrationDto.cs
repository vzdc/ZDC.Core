using System;
using ZDC.Models;

namespace ZDC.Core.Dtos
{
    public class EventRegistrationDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public Event Event { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}