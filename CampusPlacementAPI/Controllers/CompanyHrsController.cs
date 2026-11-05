using CampusPlacementAPI.DTOs.CompanyHRs;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/company-hrs")]
    public class CompanyHrsController : ControllerBase
    {
        private readonly ICompanyHRService _hrService;

        public CompanyHrsController(ICompanyHRService hrService) => _hrService = hrService;

        [HttpPost("register")]
        public async Task<ActionResult<CompanyHRResponseDto>> Register([FromBody] CompanyHRRegisterDto dto)
        {
            try
            {
                var created = await _hrService.RegisterHRAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.CompanyHRId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<CompanyHRResponseDto>>> GetAll() =>
            Ok(await _hrService.GetAllHRsAsync());

        [HttpGet("pending")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<CompanyHRResponseDto>>> GetPending() =>
            Ok(await _hrService.GetPendingHRsAsync());

        [HttpGet("me")]
        [Authorize(Roles = "CompanyHR")]
        public async Task<ActionResult<CompanyHRResponseDto>> GetCurrentHR()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var hr = await _hrService.GetHRByUserIdAsync(userId);
            return hr == null ? NotFound() : Ok(hr);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<CompanyHRResponseDto>> GetById(int id)
        {
            var hr = await _hrService.GetHRByIdAsync(id);
            return hr == null ? NotFound() : Ok(hr);
        }

        [HttpPut("{id:int}/approve")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Approve(int id)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Admin";
            return await _hrService.ApproveHRAsync(id, adminId) ? NoContent() : NotFound();
        }

        [HttpPut("{id:int}/reject")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Reject(int id) =>
            await _hrService.RejectHRAsync(id) ? NoContent() : NotFound();
    }
}