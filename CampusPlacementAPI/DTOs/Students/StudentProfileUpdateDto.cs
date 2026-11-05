using System.ComponentModel.DataAnnotations;

namespace CampusPlacementAPI.DTOs.Students
{
    public class StudentProfileUpdateDto
    {
        [Range(0, 10)]
        public decimal? CGPA { get; set; }

        [Range(0, 100)]
        public decimal? TenthPercentage { get; set; }

        [Range(0, 100)]
        public decimal? TwelfthPercentage { get; set; }

        [Url]
        public string? ResumeUrl { get; set; }

        [MaxLength(500)]
        public string? Skills { get; set; }
    }
}