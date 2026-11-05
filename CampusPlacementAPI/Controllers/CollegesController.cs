using CampusPlacementAPI.DTOs.Colleges;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/colleges")]
    public class CollegesController : ControllerBase
    {
        private readonly ICollegeService _collegeService;

        public CollegesController(ICollegeService collegeService)
        {
            _collegeService = collegeService;
        }

        // POST: api/colleges/register (Public)
        [HttpPost("register")]
        public async Task<ActionResult<CollegeResponseDto>> RegisterCollege([FromBody] CollegeRegisterDto registerDto)
        {
            try
            {
                var createdCollege = await _collegeService.RegisterCollegeAsync(registerDto);
                return CreatedAtAction(nameof(GetCollegeById), new { id = createdCollege.CollegeId }, createdCollege);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/colleges (SuperAdmin Only)
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<CollegeResponseDto>>> GetAllColleges()
        {
            var colleges = await _collegeService.GetAllCollegesAsync();
            return Ok(colleges);
        }

        // GET: api/colleges/pending (SuperAdmin Only)
        [HttpGet("pending")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<CollegeResponseDto>>> GetPendingColleges()
        {
            var pendingColleges = await _collegeService.GetPendingCollegesAsync();
            return Ok(pendingColleges);
        }

        // GET: api/colleges/{id} (SuperAdmin or Associated PlacementOfficer)
        [HttpGet("{id:int}")]
        [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
        public async Task<ActionResult<CollegeResponseDto>> GetCollegeById(int id)
        {
            var college = await _collegeService.GetCollegeByIdAsync(id);
            if (college == null)
            {
                return NotFound(new { message = $"College with ID {id} not found." });
            }
            return Ok(college);
        }

        // PUT: api/colleges/{id}/approve (SuperAdmin Only)
        [HttpPut("{id:int}/approve")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveCollege(int id)
        {
            var success = await _collegeService.ApproveCollegeAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"College with ID {id} not found." });
            }
            return NoContent();
        }

        // PUT: api/colleges/{id}/reject (SuperAdmin Only)
        [HttpPut("{id:int}/reject")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectCollege(int id)
        {
            var success = await _collegeService.RejectCollegeAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"College with ID {id} not found." });
            }
            return NoContent();
        }

        // PUT: api/colleges/{id}/suspend (SuperAdmin Only)
        [HttpPut("{id:int}/suspend")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SuspendCollege(int id)
        {
            var success = await _collegeService.SuspendCollegeAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"College with ID {id} not found." });
            }
            return NoContent();
        }

        // PUT: api/colleges/{id} (SuperAdmin Only)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateCollege(int id, [FromBody] CollegeUpdateDto updateDto)
        {
            var success = await _collegeService.UpdateCollegeAsync(id, updateDto);
            if (!success)
            {
                return NotFound(new { message = $"College with ID {id} not found." });
            }
            return NoContent();
        }
    }
}