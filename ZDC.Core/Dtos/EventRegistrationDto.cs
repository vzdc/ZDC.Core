using System;
using ZDC.Models;

namespace ZDC.Core.Dtos
{
    public class EventRegistrationDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public Event Event { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}