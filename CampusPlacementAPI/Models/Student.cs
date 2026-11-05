using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public int CollegeId { get; set; }

        [ForeignKey("CollegeId")]
        public virtual College College { get; set; } = null!;

        [Required]
        public int PlacementOfficerId { get; set; }

        [ForeignKey("PlacementOfficerId")]
        public virtual PlacementOfficer PlacementOfficer { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string RegisterNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Degree { get; set; } = string.Empty;

        [Required]
        public int GraduationYear { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,2)")]
        public decimal CGPA { get; set; }

        [Required]
        public int ActiveBacklogs { get; set; }

        [MaxLength(500)]
        public string? ResumeUrl { get; set; }

        [Required]
        public AccountStatus Status { get; set; } = AccountStatus.Pending;

        public DateTime? ApprovedAt { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}