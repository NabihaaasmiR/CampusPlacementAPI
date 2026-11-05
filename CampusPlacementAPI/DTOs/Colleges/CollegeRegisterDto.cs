using System.ComponentModel.DataAnnotations;

namespace CampusPlacementAPI.DTOs.Colleges
{
    public class CollegeRegisterDto
    {
        [Required(ErrorMessage = "College name is required.")]
        [MaxLength(200)]
        public string CollegeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "College code is required.")]
        [MaxLength(50)]
        public string CollegeCode { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? UniversityName { get; set; }

        [Required(ErrorMessage = "Official email is required.")]
        [EmailAddress]
        [MaxLength(255)]
        public string OfficialEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [Url]
        [MaxLength(255)]
        public string? Website { get; set; }
    }
}