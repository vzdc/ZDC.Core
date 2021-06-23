using System;
using System.Collections.Generic;
using ZDC.Models;

namespace ZDC.Core.Dtos
{
    public class UserStaffDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Initials { get; set; }
        public string Email { get; set; }
        public string ReverseNameCid => $"{LastName}, {FirstName} - {Id}";
        public UserRating UserRating { get; set; }

        public string RatingLong
        {
            get
            {
                return UserRating switch
                {
                    UserRating.Inactive => "Inactive",
                    UserRating.OBS => "Observer",
                    UserRating.S1 => "Student",
                    UserRating.S2 => "Student 2",
                    UserRating.S3 => "Student 3",
                    UserRating.C1 => "Controller",
                    UserRating.C2 => "Controller 2",
                    UserRating.C3 => "Senior Controller",
                    UserRating.I1 => "Instructor",
                    UserRating.I2 => "Instructor 2",
                    UserRating.I3 => "Senior Instructor",
                    UserRating.SUP => "Supervisor",
                    UserRating.ADM => "Administrator",
                    _ => "None"
                };
            }
        }

        public virtual Certification Certifications { get; set; }
        public virtual ICollection<RoleDto> Roles { get; set; }
        public bool Training { get; set; }
        public bool Events { get; set; }
        public bool Visitor { get; set; }
        public bool Currency { get; set; }
        public string VisitorFrom { get; set; }
        public UserStatus Status { get; set; }
        public DateTime Joined { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}