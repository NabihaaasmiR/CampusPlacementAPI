namespace CampusPlacementAPI.DTOs.Reports
{
    public class DepartmentStatsDto
    {
        public string Department { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int PlacedStudents { get; set; }
        public decimal PlacementPercentage { get; set; }
    }
}