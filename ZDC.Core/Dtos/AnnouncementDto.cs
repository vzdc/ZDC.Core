using System;

namespace ZDC.Core.Dtos
{
    public class AnnouncementDto
    {
        public int Id { get; set; }
        public UserDto Submitter { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}