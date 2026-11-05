using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public class RecruitmentDrive
    {
        [Key]
        public int DriveId { get; set; }

        [Required]
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual JobOpening JobOpening { get; set; } = null!;

        [Required]
        public int CollegeId { get; set; }
        [ForeignKey("CollegeId")]
        public virtual College College { get; set; } = null!;

        [Required]
        public DateTime DriveDate { get; set; }

        [MaxLength(255)]
        public string? Venue { get; set; }

        public string? Description { get; set; }

        [Required]
        public CollegeStatus Status { get; set; } = CollegeStatus.Pending;
    }
}