using CampusPlacementAPI.DTOs.Colleges;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;

namespace CampusPlacementAPI.Services
{
    public class CollegeService : ICollegeService
    {
        private readonly ICollegeRepository _collegeRepository;

        public CollegeService(ICollegeRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
        }

        public async Task<IEnumerable<CollegeResponseDto>> GetAllCollegesAsync()
        {
            var colleges = await _collegeRepository.GetAllAsync();
            return colleges.Select(MapToResponseDto);
        }

        public async Task<CollegeResponseDto?> GetCollegeByIdAsync(int collegeId)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);
            return college == null ? null : MapToResponseDto(college);
        }

        public async Task<IEnumerable<CollegeResponseDto>> GetPendingCollegesAsync()
        {
            var pendingColleges = await _collegeRepository.GetByStatusAsync(CollegeStatus.Pending);
            return pendingColleges.Select(MapToResponseDto);
        }

        public async Task<CollegeResponseDto> RegisterCollegeAsync(CollegeRegisterDto dto)
        {
            // Business rule checks
            var existingByCode = await _collegeRepository.GetByCodeAsync(dto.CollegeCode);
            if (existingByCode != null)
            {
                throw new InvalidOperationException($"A college with code '{dto.CollegeCode}' already exists.");
            }

            var existingByEmail = await _collegeRepository.GetByEmailAsync(dto.OfficialEmail);
            if (existingByEmail != null)
            {
                throw new InvalidOperationException($"A college with official email '{dto.OfficialEmail}' already exists.");
            }

            var college = new College
            {
                CollegeName = dto.CollegeName,
                CollegeCode = dto.CollegeCode,
                UniversityName = dto.UniversityName,
                OfficialEmail = dto.OfficialEmail,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Website = dto.Website,
                Status = CollegeStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var createdCollege = await _collegeRepository.AddAsync(college);
            return MapToResponseDto(createdCollege);
        }

        public async Task<bool> ApproveCollegeAsync(int collegeId)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);
            if (college == null) return false;

            college.Status = CollegeStatus.Approved;
            await _collegeRepository.UpdateAsync(college);
            return true;
        }

        public async Task<bool> RejectCollegeAsync(int collegeId)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);
            if (college == null) return false;

            college.Status = CollegeStatus.Rejected;
            await _collegeRepository.UpdateAsync(college);
            return true;
        }

        public async Task<bool> SuspendCollegeAsync(int collegeId)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);
            if (college == null) return false;

            college.Status = CollegeStatus.Suspended;
            await _collegeRepository.UpdateAsync(college);
            return true;
        }

        public async Task<bool> UpdateCollegeAsync(int collegeId, CollegeUpdateDto dto)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);
            if (college == null) return false;

            college.CollegeName = dto.CollegeName;
            college.UniversityName = dto.UniversityName;
            college.OfficialEmail = dto.OfficialEmail;
            college.PhoneNumber = dto.PhoneNumber;
            college.Address = dto.Address;
            college.Website = dto.Website;
            college.Status = dto.Status;

            await _collegeRepository.UpdateAsync(college);
            return true;
        }

        private static CollegeResponseDto MapToResponseDto(College college)
        {
            return new CollegeResponseDto
            {
                CollegeId = college.CollegeId,
                CollegeName = college.CollegeName,
                CollegeCode = college.CollegeCode,
                UniversityName = college.UniversityName,
                OfficialEmail = college.OfficialEmail,
                PhoneNumber = college.PhoneNumber,
                Address = college.Address,
                Website = college.Website,
                Status = college.Status.ToString(),
                CreatedAt = college.CreatedAt
            };
        }
    }
}