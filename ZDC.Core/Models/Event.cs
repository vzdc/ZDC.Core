using System;
using System.Collections.Generic;

namespace ZDC.Core.Models
{
    public class Event : BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Banner { get; set; }
        public string Host { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Open { get; set; }
        public IList<EventRegistration> Registrations { get; set; }
        public IList<EventPosition> Positions { get; set; }
    }

    public class EventRegistration : BaseModel
    {
        public string Position { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class EventPosition : BaseModel
    {
        public string Position { get; set; }
    }
}