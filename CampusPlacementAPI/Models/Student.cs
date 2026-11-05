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

        // Alias property to support both RegisterNumber and RegistrationNumber references
        [NotMapped]
        public string RegistrationNumber
        {
            get => RegisterNumber;
            set => RegisterNumber = value;
        }

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        // Property to support DepartmentId if used in DTOs
        public int DepartmentId { get; set; }

        [NotMapped]
        public string DepartmentName
        {
            get => Department;
            set => Department = value;
        }

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

        // Additional profile metrics
        [Column(TypeName = "decimal(5,2)")]
        public decimal? TenthPercentage { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? TwelfthPercentage { get; set; }

        [MaxLength(500)]
        public string? Skills { get; set; }

        [MaxLength(500)]
        public string? ResumeUrl { get; set; }

        [Required]
        public AccountStatus Status { get; set; } = AccountStatus.Pending;

        [MaxLength(450)]
        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}