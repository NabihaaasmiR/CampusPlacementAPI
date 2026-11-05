using CampusPlacementAPI.DTOs.Reports;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "PlacementOfficer,Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<PlacementSummaryResponseDto>> GetOverallSummary()
        {
            var result = await _reportService.GetOverallSummaryAsync();
            return Ok(result);
        }

        [HttpGet("department-stats")]
        public async Task<ActionResult<IEnumerable<DepartmentStatsDto>>> GetDepartmentStats()
        {
            var result = await _reportService.GetDepartmentStatsAsync();
            return Ok(result);
        }

        [HttpGet("company-stats")]
        public async Task<ActionResult<IEnumerable<CompanyRecruitmentStatsDto>>> GetCompanyStats()
        {
            var result = await _reportService.GetCompanyRecruitmentStatsAsync();
            return Ok(result);
        }
    }
}