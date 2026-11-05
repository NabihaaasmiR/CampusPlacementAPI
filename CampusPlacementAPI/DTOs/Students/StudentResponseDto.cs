namespace CampusPlacementAPI.DTOs.Students
{
    public class StudentResponseDto
    {
        public int StudentId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int GraduationYear { get; set; }
        public decimal? CGPA { get; set; }
        public decimal? TenthPercentage { get; set; }
        public decimal? TwelfthPercentage { get; set; }
        public string? ResumeUrl { get; set; }
        public string? Skills { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}