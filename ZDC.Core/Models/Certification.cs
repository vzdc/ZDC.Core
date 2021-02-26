namespace ZDC.Core.Models
{
    public class Certification : BaseModel
    {
        public CertificationType Ground { get; set; }
        public CertificationType Tower { get; set; }
        public CertificationType Approach { get; set; }
        public CertificationType Center { get; set; }
    }


    public enum CertificationType
    {
        None,
        Solo,
        Minor,
        Certified
    }
}