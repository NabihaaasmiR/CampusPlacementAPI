using CampusPlacementAPI.DTOs.Students;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentService(IStudentRepository studentRepository, UserManager<ApplicationUser> userManager)
        {
            _studentRepository = studentRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync() =>
            (await _studentRepository.GetAllAsync()).Select(MapToDto);

        public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student == null ? null : MapToDto(student);
        }

        public async Task<StudentResponseDto?> GetStudentByUserIdAsync(string userId)
        {
            var student = await _studentRepository.GetByUserIdAsync(userId);
            return student == null ? null : MapToDto(student);
        }

        public async Task<IEnumerable<StudentResponseDto>> GetPendingStudentsAsync() =>
            (await _studentRepository.GetByStatusAsync(AccountStatus.Pending)).Select(MapToDto);

        public async Task<StudentResponseDto> RegisterStudentAsync(StudentRegisterDto dto)
        {
            var existingRegNo = await _studentRepository.GetByRegNumberAsync(dto.RegistrationNumber);
            if (existingRegNo != null) throw new InvalidOperationException("Registration number is already in use.");

            var user = new ApplicationUser
            {
                UserName = dto.RegistrationNumber,
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                AccountStatus = AccountStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create Student account: {errors}");
            }

            var student = new Student
            {
                UserId = user.Id,
                RegisterNumber = dto.RegistrationNumber,
                DepartmentId = dto.DepartmentId,
                Department = "General", // Default or mapped department string
                GraduationYear = dto.GraduationYear
            };

            var created = await _studentRepository.AddAsync(student);
            created.User = user;

            return MapToDto(created);
        }

        public async Task<bool> ApproveStudentAsync(int id, string adminUserId)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return false;

            student.ApprovedBy = adminUserId;
            student.ApprovedAt = DateTime.UtcNow;
            student.User.AccountStatus = AccountStatus.Approved;

            await _userManager.AddToRoleAsync(student.User, "Student");
            await _studentRepository.UpdateAsync(student);
            await _userManager.UpdateAsync(student.User);

            return true;
        }

        public async Task<bool> RejectStudentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return false;

            student.User.AccountStatus = AccountStatus.Rejected;
            await _studentRepository.UpdateAsync(student);
            await _userManager.UpdateAsync(student.User);

            return true;
        }

        public async Task<bool> UpdateStudentProfileAsync(string userId, StudentProfileUpdateDto dto)
        {
            var student = await _studentRepository.GetByUserIdAsync(userId);
            if (student == null) return false;

            student.CGPA = dto.CGPA ?? student.CGPA;
            student.TenthPercentage = dto.TenthPercentage ?? student.TenthPercentage;
            student.TwelfthPercentage = dto.TwelfthPercentage ?? student.TwelfthPercentage;
            student.ResumeUrl = dto.ResumeUrl ?? student.ResumeUrl;
            student.Skills = dto.Skills ?? student.Skills;

            await _studentRepository.UpdateAsync(student);
            return true;
        }

        private static StudentResponseDto MapToDto(Student s) => new()
        {
            StudentId = s.StudentId,
            UserId = s.UserId,
            FullName = s.User?.FullName ?? string.Empty,
            Email = s.User?.Email ?? string.Empty,
            PhoneNumber = s.User?.PhoneNumber ?? string.Empty,
            RegistrationNumber = s.RegistrationNumber,
            DepartmentId = s.DepartmentId,
            DepartmentName = s.Department ?? string.Empty,
            GraduationYear = s.GraduationYear,
            CGPA = s.CGPA,
            TenthPercentage = s.TenthPercentage,
            TwelfthPercentage = s.TwelfthPercentage,
            ResumeUrl = s.ResumeUrl,
            Skills = s.Skills,
            Status = s.User?.AccountStatus.ToString() ?? AccountStatus.Pending.ToString(),
            ApprovedBy = s.ApprovedBy,
            ApprovedAt = s.ApprovedAt
        };
    }
}