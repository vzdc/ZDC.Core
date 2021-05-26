using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Certification
    {
        public Certification()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public CertificationType Ground { get; set; }
        public CertificationType Tower { get; set; }
        public CertificationType MinorApproach { get; set; }
        public CertificationType Chesapeake { get; set; }
        public CertificationType MountVernon { get; set; }
        public CertificationType Shenandoah { get; set; }
        public CertificationType Center { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }


    public enum CertificationType
    {
        None,
        Solo,
        Minor,
        Certified
    }
}