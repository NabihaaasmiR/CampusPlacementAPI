using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.DTOs.RecruitmentDrives
{
    public class RecruitmentDriveResponseDto
    {
        public int DriveId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int CollegeId { get; set; }
        public string CollegeName { get; set; } = string.Empty;
        public DateTime DriveDate { get; set; }
        public string? Venue { get; set; }
        public string? Description { get; set; }
        public DriveStatus Status { get; set; }
    }
}