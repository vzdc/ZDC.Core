using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class User
    {
        public User()
        {
            Certifications = new Certification
            {
                Ground = 0,
                Tower = 0,
                Approach = 0,
                Center = 0,
                Updated = DateTime.UtcNow
            };
        }

        [Key] public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Initials { get; set; }
        public string Email { get; set; }
        public string ReverseNameCid => $"{LastName}, {FirstName} - {Id}";
        public UserRating UserRating { get; set; }
        public Certification Certifications { get; set; }
        public IList<Loas> Loas { get; set; }
        public IList<Warnings> Warnings { get; set; }
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

    public class Certification
    {
        [Key] public int Id { get; set; }

        public CertificationType Ground { get; set; }
        public CertificationType Tower { get; set; }
        public CertificationType Approach { get; set; }
        public CertificationType Center { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Loas
    {
        [Key] public int Id { get; set; }

        public string Reason { get; set; }
        public string MoreInfo { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public LoaStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Warnings
    {
        [Key] public int Id { get; set; }
        public WarningReason Reason { get; set; }
        public WarningStatus Status { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
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

    public enum CertificationType
    {
        None,
        Solo,
        Minor,
        Certified
    }

    public enum LoaStatus
    {
        Pending,
        MoreInfo,
        Accepted,
        Started,
        Canceled,
        Ended,
        Controlled
    }

    public enum WarningReason
    {
        Activity,
        Grp
    }

    public enum WarningStatus
    {
        Warned,
        GotFirstHalf,
        Resolved,
        NeedsRemoval
    }
}