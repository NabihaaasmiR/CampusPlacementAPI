using CampusPlacementAPI.DTOs.JobApplications;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _applicationService;

        public JobApplicationsController(IJobApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<JobApplicationResponseDto>> Apply([FromBody] JobApplicationCreateDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var result = await _applicationService.ApplyForJobAsync(userId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-applications")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>> GetMyApplications()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var result = await _applicationService.GetApplicationsForStudentAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("job/{jobId}")]
        [Authorize(Roles = "CompanyHR,PlacementOfficer,Admin")]
        public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>> GetApplicationsForJob(int jobId)
        {
            try
            {
                var result = await _applicationService.GetApplicationsForJobAsync(jobId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{applicationId}/status")]
        [Authorize(Roles = "CompanyHR,PlacementOfficer,Admin")]
        public async Task<ActionResult<JobApplicationResponseDto>> UpdateStatus(int applicationId, [FromBody] JobApplicationStatusUpdateDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var result = await _applicationService.UpdateApplicationStatusAsync(applicationId, userId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}