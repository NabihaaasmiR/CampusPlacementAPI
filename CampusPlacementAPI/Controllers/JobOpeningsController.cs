using CampusPlacementAPI.DTOs.JobOpenings;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/job-openings")]
    public class JobOpeningsController : ControllerBase
    {
        private readonly IJobOpeningService _jobService;
        private readonly ICompanyHRService _hrService;

        public JobOpeningsController(IJobOpeningService jobService, ICompanyHRService hrService)
        {
            _jobService = jobService;
            _hrService = hrService;
        }

        // GET: api/job-openings/published (Public / Students)
        [HttpGet("published")]
        public async Task<ActionResult<IEnumerable<JobOpeningResponseDto>>> GetPublishedJobs() =>
            Ok(await _jobService.GetPublishedJobsAsync());

        // GET: api/job-openings/{id} (Authenticated)
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<JobOpeningResponseDto>> GetById(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            return job == null ? NotFound(new { message = "Job opening not found." }) : Ok(job);
        }

        // POST: api/job-openings (CompanyHR Only)
        [HttpPost]
        [Authorize(Roles = "CompanyHR")]
        public async Task<ActionResult<JobOpeningResponseDto>> CreateJob([FromBody] JobOpeningCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var hrProfile = await _hrService.GetHRByUserIdAsync(userId);
            if (hrProfile == null) return BadRequest(new { message = "HR profile not found." });

            var created = await _jobService.CreateJobAsync(hrProfile.CompanyId, hrProfile.CompanyHRId, dto);
            return CreatedAtAction(nameof(GetById), new { id = created.JobId }, created);
        }

        // PUT: api/job-openings/{id}/publish (CompanyHR Only)
        [HttpPut("{id:int}/publish")]
        [Authorize(Roles = "CompanyHR")]
        public async Task<IActionResult> PublishJob(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var hrProfile = await _hrService.GetHRByUserIdAsync(userId);
            if (hrProfile == null) return BadRequest();

            var success = await _jobService.PublishJobAsync(id, hrProfile.CompanyId);
            return success ? NoContent() : Forbid();
        }

        // PUT: api/job-openings/{id}/close (CompanyHR Only)
        [HttpPut("{id:int}/close")]
        [Authorize(Roles = "CompanyHR")]
        public async Task<IActionResult> CloseJob(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var hrProfile = await _hrService.GetHRByUserIdAsync(userId);
            if (hrProfile == null) return BadRequest();

            var success = await _jobService.CloseJobAsync(id, hrProfile.CompanyId);
            return success ? NoContent() : Forbid();
        }
    }
}