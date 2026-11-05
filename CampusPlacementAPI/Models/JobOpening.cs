using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public enum JobStatus
    {
        Draft,
        Published,
        Closed,
        Cancelled
    }

    public class JobOpening
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;

        [Required]
        public int CreatedByHRId { get; set; }
        [ForeignKey("CreatedByHRId")]
        public virtual CompanyHR CreatedByHR { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public string JobDescription { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string JobType { get; set; } = string.Empty; // FullTime, Internship, etc.

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [Column(TypeName = "decimal(12,2)")]
        public decimal? SalaryPackage { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? MinimumCGPA { get; set; }

        public int? MaximumBacklogs { get; set; }

        [Required]
        public DateTime ApplicationDeadline { get; set; }

        [Required]
        public bool IsOpenToAllColleges { get; set; } = true;

        [Required]
        public JobStatus Status { get; set; } = JobStatus.Draft;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<JobTargetCollege> JobTargetColleges { get; set; } = new List<JobTargetCollege>();
        public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
        public virtual ICollection<RecruitmentDrive> RecruitmentDrives { get; set; } = new List<RecruitmentDrive>();
        public virtual ICollection<InterviewRound> InterviewRounds { get; set; } = new List<InterviewRound>();
    }
}