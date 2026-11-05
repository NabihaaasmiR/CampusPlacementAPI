using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public enum ApplicationStatus
    {
        Applied,
        UnderReview,
        Shortlisted,
        Rejected,
        InterviewScheduled,
        Selected,
        NotSelected,
        Withdrawn
    }

    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual JobOpening JobOpening { get; set; } = null!;

        [Required]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        [Required]
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

        [MaxLength(500)]
        public string? ResumeUrl { get; set; }

        [Required]
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<InterviewSchedule> InterviewSchedules { get; set; } = new List<InterviewSchedule>();
        public virtual ICollection<ApplicationStatusHistory> StatusHistories { get; set; } = new List<ApplicationStatusHistory>();
    }
}