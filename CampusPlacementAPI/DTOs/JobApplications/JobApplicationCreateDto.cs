using System.ComponentModel.DataAnnotations;

namespace CampusPlacementAPI.DTOs.JobApplications
{
    public class JobApplicationCreateDto
    {
        [Required]
        public int JobId { get; set; }

        [MaxLength(500)]
        public string? ResumeUrl { get; set; }
    }
}