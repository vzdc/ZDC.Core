using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Dossier
    {
        public Dossier()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public virtual User Submitter { get; set; }
        public string Text { get; set; }
        public bool Confidential { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}