using System.ComponentModel.DataAnnotations;

namespace CampusPlacementAPI.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string CompanyEmail { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Website { get; set; }

        [Required]
        [MaxLength(100)]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public CollegeStatus Status { get; set; } = CollegeStatus.Pending;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<CompanyHR> CompanyHRs { get; set; } = new List<CompanyHR>();
        public virtual ICollection<JobOpening> JobOpenings { get; set; } = new List<JobOpening>();
    }
}