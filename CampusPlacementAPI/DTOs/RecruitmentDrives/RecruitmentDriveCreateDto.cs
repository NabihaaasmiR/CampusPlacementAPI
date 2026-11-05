using System.ComponentModel.DataAnnotations;

namespace CampusPlacementAPI.DTOs.RecruitmentDrives
{
    public class RecruitmentDriveCreateDto
    {
        [Required]
        public int JobId { get; set; }

        [Required]
        public int CollegeId { get; set; }

        [Required]
        public DateTime DriveDate { get; set; }

        [MaxLength(255)]
        public string? Venue { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}