using System.ComponentModel.DataAnnotations;

namespace CampusPlacementAPI.DTOs.Interviews
{
    public class InterviewRoundCreateDto
    {
        [Required]
        public int JobId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoundName { get; set; } = string.Empty;

        [Required]
        public int RoundNumber { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}