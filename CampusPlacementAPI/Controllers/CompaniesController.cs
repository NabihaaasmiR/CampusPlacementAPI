using CampusPlacementAPI.DTOs.Companies;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService) => _companyService = companyService;

        [HttpPost("register")]
        public async Task<ActionResult<CompanyResponseDto>> Register([FromBody] CompanyRegisterDto dto)
        {
            try
            {
                var created = await _companyService.RegisterCompanyAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.CompanyId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<CompanyResponseDto>>> GetAll() =>
            Ok(await _companyService.GetAllCompaniesAsync());

        [HttpGet("pending")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<CompanyResponseDto>>> GetPending() =>
            Ok(await _companyService.GetPendingCompaniesAsync());

        [HttpGet("{id:int}")]
        [Authorize(Roles = "SuperAdmin,CompanyHR")]
        public async Task<ActionResult<CompanyResponseDto>> GetById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            return company == null ? NotFound(new { message = "Company not found." }) : Ok(company);
        }

        [HttpPut("{id:int}/approve")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Approve(int id) =>
            await _companyService.ApproveCompanyAsync(id) ? NoContent() : NotFound();

        [HttpPut("{id:int}/reject")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Reject(int id) =>
            await _companyService.RejectCompanyAsync(id) ? NoContent() : NotFound();

        [HttpPut("{id:int}/suspend")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Suspend(int id) =>
            await _companyService.SuspendCompanyAsync(id) ? NoContent() : NotFound();

        [HttpPut("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyUpdateDto dto) =>
            await _companyService.UpdateCompanyAsync(id, dto) ? NoContent() : NotFound();
    }
}