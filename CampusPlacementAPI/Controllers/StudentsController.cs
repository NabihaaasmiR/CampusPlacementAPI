using CampusPlacementAPI.DTOs.Students;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusPlacementAPI.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService) => _studentService = studentService;

        [HttpPost("register")]
        public async Task<ActionResult<StudentResponseDto>> Register([FromBody] StudentRegisterDto dto)
        {
            try
            {
                var created = await _studentService.RegisterStudentAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.StudentId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,CollegeAdmin,CompanyHR")]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAll() =>
            Ok(await _studentService.GetAllStudentsAsync());

        [HttpGet("pending")]
        [Authorize(Roles = "SuperAdmin,CollegeAdmin")]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetPending() =>
            Ok(await _studentService.GetPendingStudentsAsync());

        [HttpGet("me")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<StudentResponseDto>> GetCurrentStudent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var student = await _studentService.GetStudentByUserIdAsync(userId);
            return student == null ? NotFound() : Ok(student);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "SuperAdmin,CollegeAdmin,CompanyHR")]
        public async Task<ActionResult<StudentResponseDto>> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            return student == null ? NotFound() : Ok(student);
        }

        [HttpPut("{id:int}/approve")]
        [Authorize(Roles = "SuperAdmin,CollegeAdmin")]
        public async Task<IActionResult> Approve(int id)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Admin";
            return await _studentService.ApproveStudentAsync(id, adminId) ? NoContent() : NotFound();
        }

        [HttpPut("{id:int}/reject")]
        [Authorize(Roles = "SuperAdmin,CollegeAdmin")]
        public async Task<IActionResult> Reject(int id) =>
            await _studentService.RejectStudentAsync(id) ? NoContent() : NotFound();

        [HttpPut("me/profile")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateProfile([FromBody] StudentProfileUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            return await _studentService.UpdateStudentProfileAsync(userId, dto) ? NoContent() : NotFound();
        }
    }
}