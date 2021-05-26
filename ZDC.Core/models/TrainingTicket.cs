using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class TrainingTicket
    {
        public TrainingTicket()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public virtual User Student { get; set; }
        public virtual User Trainer { get; set; }
        public TrainingTicketPosition Position { get; set; }
        public TrainingTicketFacility Facility { get; set; }
        public TrainingTicketType Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration => End - Start;
        public string StudentComments { get; set; }
        public string TrainerComments { get; set; }
        public bool NoShow { get; set; }
        public bool OtsRecommendation { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum TrainingTicketPosition
    {
        MinorGround,
        MinorLocal,
        MajorGround,
        MajorLocal,
        MinorApproach,
        MajorApproach,
        Center
    }

    public enum TrainingTicketFacility
    {
        KIAD,
        KBWI,
        KDCA,
        KORF,
        DC
    }

    public enum TrainingTicketType
    {
        Classroom,
        Sweatbox,
        Network,
        Monitoring,
        SweatboxOTSPass,
        NetworkOTSPass,
        SweatboxOTSFail,
        NetworkOTSFail,
        OTS,
        NA
    }
}