using System.ComponentModel.DataAnnotations;

namespace CampusPlacementAPI.Models
{
    public enum CollegeStatus
    {
        Pending,
        Approved,
        Rejected,
        Suspended,
        Inactive
    }

    public class College
    {
        [Key]
        public int CollegeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CollegeName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string CollegeCode { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? UniversityName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string OfficialEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Website { get; set; }

        [Required]
        public CollegeStatus Status { get; set; } = CollegeStatus.Pending;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<PlacementOfficer> PlacementOfficers { get; set; } = new List<PlacementOfficer>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<JobTargetCollege> JobTargetColleges { get; set; } = new List<JobTargetCollege>();
    }
}