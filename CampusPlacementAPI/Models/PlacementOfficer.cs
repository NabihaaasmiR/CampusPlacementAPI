using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementAPI.Models
{
    public class PlacementOfficer
    {
        [Key]
        public int PlacementOfficerId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public int CollegeId { get; set; }

        [ForeignKey("CollegeId")]
        public virtual College College { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        public AccountStatus Status { get; set; } = AccountStatus.Pending;

        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public virtual ICollection<Student> EnrolledStudents { get; set; } = new List<Student>();
    }
}