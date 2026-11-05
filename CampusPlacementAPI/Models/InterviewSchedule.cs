using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public enum InterviewStatus
    {
        Scheduled,
        Rescheduled,
        Completed,
        Cancelled,
        Passed,
        Failed
    }

    public class InterviewRound
    {
        [Key]
        public int InterviewRoundId { get; set; }

        [Required]
        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public virtual JobOpening JobOpening { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string RoundName { get; set; } = string.Empty;

        [Required]
        public int RoundNumber { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<InterviewSchedule> InterviewSchedules { get; set; } = new List<InterviewSchedule>();
    }

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
        public InterviewStatus Status { get; set; } = InterviewStatus.Scheduled;

        public string? Feedback { get; set; }
    }
}