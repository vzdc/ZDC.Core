using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class User
    {
        public User()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Initials { get; set; }
        public string Email { get; set; }
        public string ReverseNameCid => $"{LastName}, {FirstName} - {Id}";
        public UserRating UserRating { get; set; }
        public virtual Certification Certifications { get; set; }
        public virtual IList<Loa> Loas { get; set; }
        public virtual IList<Warning> Warnings { get; set; }
        public virtual IList<Dossier> DossierEntries { get; set; }
        public virtual IList<Feedback> Feedback { get; set; }
        public virtual IList<Hours> Hours { get; set; }
        public UserRole Role { get; set; }
        public TrainingRole TrainingRole { get; set; }
        public bool Training { get; set; }
        public bool Events { get; set; }
        public bool Visitor { get; set; }
        public string VisitorFrom { get; set; }
        public UserStatus Status { get; set; }
        public DateTime Joined { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum UserRating
    {
        OBS = 1,
        S1,
        S2,
        S3,
        C1,
        C3 = 7,
        I1,
        I3 = 10,
        SUP,
        ADM
    }

    public enum UserRole
    {
        ATM,
        DATM,
        TA,
        ATA,
        WM,
        AWM,
        EC,
        AEC,
        FE,
        AFE,
        None
    }

    public enum TrainingRole
    {
        INS,
        MTR,
        None
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Loa,
        Removed
    }
}