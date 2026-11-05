using CampusPlacementAPI.DTOs.PlacementOfficers;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementAPI.Services
{
    public class PlacementOfficerService : IPlacementOfficerService
    {
        private readonly IPlacementOfficerRepository _officerRepository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlacementOfficerService(
            IPlacementOfficerRepository officerRepository,
            ICollegeRepository collegeRepository,
            UserManager<ApplicationUser> userManager)
        {
            _officerRepository = officerRepository;
            _collegeRepository = collegeRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<PlacementOfficerResponseDto>> GetAllOfficersAsync()
        {
            var officers = await _officerRepository.GetAllAsync();
            return officers.Select(MapToResponseDto);
        }

        public async Task<PlacementOfficerResponseDto?> GetOfficerByIdAsync(int id)
        {
            var officer = await _officerRepository.GetByIdAsync(id);
            return officer == null ? null : MapToResponseDto(officer);
        }

        public async Task<PlacementOfficerResponseDto?> GetOfficerByUserIdAsync(string userId)
        {
            var officer = await _officerRepository.GetByUserIdAsync(userId);
            return officer == null ? null : MapToResponseDto(officer);
        }

        public async Task<IEnumerable<PlacementOfficerResponseDto>> GetPendingOfficersAsync()
        {
            var pendingOfficers = await _officerRepository.GetByStatusAsync(AccountStatus.Pending);
            return pendingOfficers.Select(MapToResponseDto);
        }

        public async Task<PlacementOfficerResponseDto> RegisterOfficerAsync(PlacementOfficerRegisterDto dto)
        {
            var college = await _collegeRepository.GetByIdAsync(dto.CollegeId);
            if (college == null || college.Status != CollegeStatus.Approved)
            {
                throw new InvalidOperationException("The selected college must be approved and active before registering a placement officer.");
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
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
                throw new InvalidOperationException($"Failed to create user account: {errors}");
            }

            var officer = new PlacementOfficer
            {
                UserId = user.Id,
                CollegeId = dto.CollegeId,
                Designation = dto.Designation,
                Status = AccountStatus.Pending
            };

            var createdOfficer = await _officerRepository.AddAsync(officer);
            createdOfficer.User = user;
            createdOfficer.College = college;

            return MapToResponseDto(createdOfficer);
        }

        public async Task<bool> ApproveOfficerAsync(int id, string adminUserId)
        {
            var officer = await _officerRepository.GetByIdAsync(id);
            if (officer == null) return false;

            officer.Status = AccountStatus.Approved;
            officer.ApprovedBy = adminUserId;
            officer.ApprovedAt = DateTime.UtcNow;
            officer.User.AccountStatus = AccountStatus.Approved;

            await _userManager.AddToRoleAsync(officer.User, "PlacementOfficer");
            await _officerRepository.UpdateAsync(officer);
            await _userManager.UpdateAsync(officer.User);

            return true;
        }

        public async Task<bool> RejectOfficerAsync(int id)
        {
            var officer = await _officerRepository.GetByIdAsync(id);
            if (officer == null) return false;

            officer.Status = AccountStatus.Rejected;
            officer.User.AccountStatus = AccountStatus.Rejected;

            await _officerRepository.UpdateAsync(officer);
            await _userManager.UpdateAsync(officer.User);
            return true;
        }

        public async Task<bool> SuspendOfficerAsync(int id)
        {
            var officer = await _officerRepository.GetByIdAsync(id);
            if (officer == null) return false;

            officer.Status = AccountStatus.Suspended;
            officer.User.AccountStatus = AccountStatus.Suspended;

            await _officerRepository.UpdateAsync(officer);
            await _userManager.UpdateAsync(officer.User);
            return true;
        }

        private static PlacementOfficerResponseDto MapToResponseDto(PlacementOfficer officer)
        {
            return new PlacementOfficerResponseDto
            {
                PlacementOfficerId = officer.PlacementOfficerId,
                UserId = officer.UserId,
                FullName = officer.User?.FullName ?? string.Empty,
                Email = officer.User?.Email ?? string.Empty,
                PhoneNumber = officer.User?.PhoneNumber ?? string.Empty,
                CollegeId = officer.CollegeId,
                CollegeName = officer.College?.CollegeName ?? string.Empty,
                Designation = officer.Designation,
                Status = officer.Status.ToString(),
                ApprovedBy = officer.ApprovedBy,
                ApprovedAt = officer.ApprovedAt
            };
        }
    }
}