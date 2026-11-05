using System.ComponentModel.DataAnnotations;

namespace CampusPlacementAPI.DTOs.JobOpenings
{
    public class JobOpeningCreateDto
    {
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

        [Range(0, 10000000)]
        public decimal? SalaryPackage { get; set; }

        [Range(0, 10)]
        public decimal? MinimumCGPA { get; set; }

        [Range(0, 20)]
        public int? MaximumBacklogs { get; set; }

        [Required]
        public DateTime ApplicationDeadline { get; set; }

        [Required]
        public bool IsOpenToAllColleges { get; set; } = true;

        public List<int>? TargetCollegeIds { get; set; }
    }
}