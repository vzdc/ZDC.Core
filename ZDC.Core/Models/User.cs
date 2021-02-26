using System;
using System.Collections.Generic;

namespace ZDC.Core.Models
{
    public class User : BaseModel
    {
        public User()
        {
            Certifications = new Certification();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Initials { get; set; }
        public string Email { get; set; }
        public string ReverseNameCid => $"{LastName}, {FirstName} - {Id}";
        public UserRating UserRating { get; set; }
        public Certification Certifications { get; set; }
        public IList<Loa> Loas { get; set; }
        public IList<Warning> Warnings { get; set; }
        public IList<ControllerLog> ControllerLogs { get; set; }
        public IList<Dossier> DossierEntries { get; set; }
        public IList<Feedback> Feedback { get; set; }
        public UserRole Role { get; set; }
        public TrainingRole TrainingRole { get; set; }
        public bool Training { get; set; }
        public bool Events { get; set; }
        public bool Visitor { get; set; }
        public string VisitorFrom { get; set; }
        public UserStatus Status { get; set; }
        public DateTime Joined { get; set; }
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