namespace CampusPlacementAPI.DTOs.Reports
{
    public class CompanyRecruitmentStatsDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int TotalJobOpenings { get; set; }
        public int TotalOffersMade { get; set; }
    }
}