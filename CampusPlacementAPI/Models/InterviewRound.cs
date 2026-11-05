using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public class InterviewRound
    {
        [Key]
        public int InterviewRoundId { get; set; }

        [Required]
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual JobOpening JobOpening { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string RoundName { get; set; } = string.Empty;

        [Required]
        public int RoundNumber { get; set; }

        public string? Description { get; set; }
    }
}