using System.ComponentModel.DataAnnotations;
using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.DTOs.JobApplications
{
    public class JobApplicationStatusUpdateDto
    {
        [Required]
        public ApplicationStatus NewStatus { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }
    }
}