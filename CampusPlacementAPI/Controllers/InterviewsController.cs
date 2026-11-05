using System.Security.Claims;
using CampusPlacementAPI.DTOs.Interviews;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _interviewService;

        public InterviewsController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [HttpPost("rounds")]
        [Authorize(Roles = "CompanyHR,Admin")]
        public async Task<ActionResult<InterviewRound>> CreateRound([FromBody] InterviewRoundCreateDto dto)
        {
            var result = await _interviewService.CreateRoundAsync(dto);
            return Ok(result);
        }

        [HttpGet("rounds/job/{jobId}")]
        public async Task<ActionResult<IEnumerable<InterviewRound>>> GetRoundsForJob(int jobId)
        {
            var result = await _interviewService.GetRoundsForJobAsync(jobId);
            return Ok(result);
        }

        [HttpPost("schedule")]
        [Authorize(Roles = "CompanyHR,PlacementOfficer,Admin")]
        public async Task<ActionResult<InterviewScheduleResponseDto>> ScheduleInterview([FromBody] InterviewScheduleCreateDto dto)
        {
            var result = await _interviewService.ScheduleInterviewAsync(dto);
            return Ok(result);
        }

        [HttpPut("schedule/{scheduleId}")]
        [Authorize(Roles = "CompanyHR,PlacementOfficer,Admin")]
        public async Task<ActionResult<InterviewScheduleResponseDto>> UpdateSchedule(int scheduleId, [FromBody] InterviewScheduleUpdateDto dto)
        {
            var result = await _interviewService.UpdateInterviewScheduleAsync(scheduleId, dto);
            return Ok(result);
        }

        [HttpGet("my-schedules")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<InterviewScheduleResponseDto>>> GetMySchedules()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated." });

            var result = await _interviewService.GetSchedulesForStudentAsync(userId);
            return Ok(result);
        }
    }
}