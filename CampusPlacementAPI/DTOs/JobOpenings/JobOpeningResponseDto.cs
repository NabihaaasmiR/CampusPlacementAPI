namespace CampusPlacementAPI.DTOs.JobOpenings
{
    public class JobOpeningResponseDto
    {
        public int JobId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal? SalaryPackage { get; set; }
        public decimal? MinimumCGPA { get; set; }
        public int? MaximumBacklogs { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public bool IsOpenToAllColleges { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<string> TargetColleges { get; set; } = new List<string>();
    }
}