using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Event
    {
        public Event()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Banner { get; set; }
        public string Host { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Open { get; set; }
        public virtual IList<EventRegistration> Registrations { get; set; }
        public virtual IList<EventPosition> Positions { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class EventRegistration
    {
        public EventRegistration()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string Position { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class EventPosition
    {
        public EventPosition()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string Position { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}