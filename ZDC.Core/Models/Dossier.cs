namespace ZDC.Core.Models
{
    public class Dossier : BaseModel
    {
        public User Submitter { get; set; }
        public string Text { get; set; }
        public bool Confidential { get; set; }
    }
}