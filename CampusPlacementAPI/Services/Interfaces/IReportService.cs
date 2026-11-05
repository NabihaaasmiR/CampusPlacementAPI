using CampusPlacementAPI.DTOs.Reports;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface IReportService
    {
        Task<PlacementSummaryResponseDto> GetOverallSummaryAsync();
        Task<IEnumerable<DepartmentStatsDto>> GetDepartmentStatsAsync();
        Task<IEnumerable<CompanyRecruitmentStatsDto>> GetCompanyRecruitmentStatsAsync();
    }
}