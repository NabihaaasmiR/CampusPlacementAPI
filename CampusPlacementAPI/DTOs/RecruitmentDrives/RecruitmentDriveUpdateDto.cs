using System.ComponentModel.DataAnnotations;
using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.DTOs.RecruitmentDrives
{
    public class RecruitmentDriveUpdateDto
    {
        [Required]
        public DateTime DriveDate { get; set; }

        [MaxLength(255)]
        public string? Venue { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DriveStatus Status { get; set; }
    }
}