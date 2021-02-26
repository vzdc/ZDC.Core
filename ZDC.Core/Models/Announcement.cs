namespace ZDC.Core.Models
{
    public class Announcement : BaseModel
    {
        public User User { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}