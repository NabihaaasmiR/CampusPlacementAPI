using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public class ApplicationStatusHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual JobApplication JobApplication { get; set; } = null!;

        public ApplicationStatus? PreviousStatus { get; set; }

        [Required]
        public ApplicationStatus NewStatus { get; set; }

        [Required]
        public string UpdatedByUserId { get; set; } = string.Empty;
        [ForeignKey("UpdatedByUserId")]
        public virtual ApplicationUser UpdatedByUser { get; set; } = null!;

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}