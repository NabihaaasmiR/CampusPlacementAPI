using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public class InterviewSchedule
    {
        [Key]
        public int InterviewScheduleId { get; set; }

        [Required]
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual JobApplication JobApplication { get; set; } = null!;

        [Required]
        public int InterviewRoundId { get; set; }
        [ForeignKey("InterviewRoundId")]
        public virtual InterviewRound InterviewRound { get; set; } = null!;

        [Required]
        public DateTime ScheduledAt { get; set; }

        [MaxLength(500)]
        public string? MeetingLink { get; set; }

        [MaxLength(255)]
        public string? Venue { get; set; }

        [Required]
        public CollegeStatus Status { get; set; } = CollegeStatus.Pending;

        public string? Feedback { get; set; }
    }
}