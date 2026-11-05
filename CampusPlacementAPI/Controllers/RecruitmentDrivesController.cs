using CampusPlacementAPI.DTOs.RecruitmentDrives;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecruitmentDrivesController : ControllerBase
    {
        private readonly IRecruitmentDriveService _driveService;

        public RecruitmentDrivesController(IRecruitmentDriveService driveService)
        {
            _driveService = driveService;
        }

        [HttpPost]
        [Authorize(Roles = "CompanyHR,PlacementOfficer,Admin")]
        public async Task<ActionResult<RecruitmentDriveResponseDto>> Create([FromBody] RecruitmentDriveCreateDto dto)
        {
            try
            {
                var result = await _driveService.CreateDriveAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{driveId}")]
        public async Task<ActionResult<RecruitmentDriveResponseDto>> GetById(int driveId)
        {
            var result = await _driveService.GetDriveByIdAsync(driveId);
            if (result == null)
                return NotFound(new { message = "Recruitment drive not found." });

            return Ok(result);
        }

        [HttpGet("job/{jobId}")]
        public async Task<ActionResult<IEnumerable<RecruitmentDriveResponseDto>>> GetByJob(int jobId)
        {
            var result = await _driveService.GetDrivesForJobAsync(jobId);
            return Ok(result);
        }

        [HttpGet("college/{collegeId}")]
        public async Task<ActionResult<IEnumerable<RecruitmentDriveResponseDto>>> GetByCollege(int collegeId)
        {
            var result = await _driveService.GetDrivesForCollegeAsync(collegeId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecruitmentDriveResponseDto>>> GetAll()
        {
            var result = await _driveService.GetAllDrivesAsync();
            return Ok(result);
        }

        [HttpPut("{driveId}")]
        [Authorize(Roles = "CompanyHR,PlacementOfficer,Admin")]
        public async Task<ActionResult<RecruitmentDriveResponseDto>> Update(int driveId, [FromBody] RecruitmentDriveUpdateDto dto)
        {
            try
            {
                var result = await _driveService.UpdateDriveAsync(driveId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{driveId}")]
        [Authorize(Roles = "CompanyHR,Admin")]
        public async Task<IActionResult> Delete(int driveId)
        {
            try
            {
                await _driveService.DeleteDriveAsync(driveId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}