using CampusPlacementAPI.DTOs.PlacementOfficers;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/placement-officers")]
    public class PlacementOfficersController : ControllerBase
    {
        private readonly IPlacementOfficerService _officerService;

        public PlacementOfficersController(IPlacementOfficerService officerService)
        {
            _officerService = officerService;
        }

        // POST: api/placement-officers/register (Public)
        [HttpPost("register")]
        public async Task<ActionResult<PlacementOfficerResponseDto>> Register([FromBody] PlacementOfficerRegisterDto dto)
        {
            try
            {
                var result = await _officerService.RegisterOfficerAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.PlacementOfficerId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/placement-officers (SuperAdmin Only)
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<PlacementOfficerResponseDto>>> GetAll()
        {
            var officers = await _officerService.GetAllOfficersAsync();
            return Ok(officers);
        }

        // GET: api/placement-officers/pending (SuperAdmin Only)
        [HttpGet("pending")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<PlacementOfficerResponseDto>>> GetPending()
        {
            var pending = await _officerService.GetPendingOfficersAsync();
            return Ok(pending);
        }

        // GET: api/placement-officers/me (PlacementOfficer Only)
        [HttpGet("me")]
        [Authorize(Roles = "PlacementOfficer")]
        public async Task<ActionResult<PlacementOfficerResponseDto>> GetCurrentOfficer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var officer = await _officerService.GetOfficerByUserIdAsync(userId);
            if (officer == null) return NotFound(new { message = "Officer profile not found." });

            return Ok(officer);
        }

        // GET: api/placement-officers/{id} (SuperAdmin or Own Officer)
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<PlacementOfficerResponseDto>> GetById(int id)
        {
            var officer = await _officerService.GetOfficerByIdAsync(id);
            if (officer == null) return NotFound(new { message = $"Placement officer with ID {id} not found." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("SuperAdmin");

            if (!isAdmin && officer.UserId != userId)
            {
                return Forbid();
            }

            return Ok(officer);
        }

        // PUT: api/placement-officers/{id}/approve (SuperAdmin Only)
        [HttpPut("{id:int}/approve")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Approve(int id)
        {
            var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Admin";
            var success = await _officerService.ApproveOfficerAsync(id, adminUserId);
            if (!success) return NotFound(new { message = $"Placement officer with ID {id} not found." });

            return NoContent();
        }

        // PUT: api/placement-officers/{id}/reject (SuperAdmin Only)
        [HttpPut("{id:int}/reject")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _officerService.RejectOfficerAsync(id);
            if (!success) return NotFound(new { message = $"Placement officer with ID {id} not found." });

            return NoContent();
        }

        // PUT: api/placement-officers/{id}/suspend (SuperAdmin Only)
        [HttpPut("{id:int}/suspend")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Suspend(int id)
        {
            var success = await _officerService.SuspendOfficerAsync(id);
            if (!success) return NotFound(new { message = $"Placement officer with ID {id} not found." });

            return NoContent();
        }
    }
}