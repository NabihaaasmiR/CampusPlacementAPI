using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public class JobTargetCollege
    {
        [Key]
        public int JobTargetCollegeId { get; set; }

        [Required]
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual JobOpening JobOpening { get; set; } = null!;

        [Required]
        public int CollegeId { get; set; }
        [ForeignKey("CollegeId")]
        public virtual College College { get; set; } = null!;
    }
}