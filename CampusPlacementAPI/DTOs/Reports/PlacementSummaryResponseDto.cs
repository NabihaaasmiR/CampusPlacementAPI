namespace CampusPlacementAPI.DTOs.Reports
{
    public class PlacementSummaryResponseDto
    {
        public int TotalStudents { get; set; }
        public int PlacedStudents { get; set; }
        public int UnplacedStudents { get; set; }
        public decimal OverallPlacementPercentage { get; set; }
        public int TotalJobOpenings { get; set; }
        public int TotalCompaniesRecruiting { get; set; }
        public decimal AverageSalaryPackage { get; set; }
    }
}