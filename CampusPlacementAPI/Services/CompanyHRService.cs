using CampusPlacementAPI.DTOs.CompanyHRs;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementAPI.Services
{
    public class CompanyHRService : ICompanyHRService
    {
        private readonly ICompanyHRRepository _hrRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyHRService(ICompanyHRRepository hrRepository, ICompanyRepository companyRepository, UserManager<ApplicationUser> userManager)
        {
            _hrRepository = hrRepository;
            _companyRepository = companyRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CompanyHRResponseDto>> GetAllHRsAsync() =>
            (await _hrRepository.GetAllAsync()).Select(MapToDto);

        public async Task<CompanyHRResponseDto?> GetHRByIdAsync(int id)
        {
            var hr = await _hrRepository.GetByIdAsync(id);
            return hr == null ? null : MapToDto(hr);
        }

        public async Task<CompanyHRResponseDto?> GetHRByUserIdAsync(string userId)
        {
            var hr = await _hrRepository.GetByUserIdAsync(userId);
            return hr == null ? null : MapToDto(hr);
        }

        public async Task<IEnumerable<CompanyHRResponseDto>> GetPendingHRsAsync() =>
            (await _hrRepository.GetByStatusAsync(AccountStatus.Pending)).Select(MapToDto);

        public async Task<CompanyHRResponseDto> RegisterHRAsync(CompanyHRRegisterDto dto)
        {
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId);
            if (company == null || company.Status != CollegeStatus.Approved)
            {
                throw new InvalidOperationException("Company must be approved and active before registering HR.");
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
                throw new InvalidOperationException($"Failed to create HR user account: {errors}");
            }

            var hr = new CompanyHR
            {
                UserId = user.Id,
                CompanyId = dto.CompanyId,
                Designation = dto.Designation
            };

            var created = await _hrRepository.AddAsync(hr);
            created.User = user;
            created.Company = company;

            return MapToDto(created);
        }

        public async Task<bool> ApproveHRAsync(int id, string adminUserId)
        {
            var hr = await _hrRepository.GetByIdAsync(id);
            if (hr == null) return false;

            hr.ApprovedBy = adminUserId;
            hr.ApprovedAt = DateTime.UtcNow;
            hr.User.AccountStatus = AccountStatus.Approved;

            await _userManager.AddToRoleAsync(hr.User, "CompanyHR");
            await _hrRepository.UpdateAsync(hr);
            await _userManager.UpdateAsync(hr.User);
            return true;
        }

        public async Task<bool> RejectHRAsync(int id)
        {
            var hr = await _hrRepository.GetByIdAsync(id);
            if (hr == null) return false;

            hr.User.AccountStatus = AccountStatus.Rejected;
            await _hrRepository.UpdateAsync(hr);
            await _userManager.UpdateAsync(hr.User);
            return true;
        }

        private static CompanyHRResponseDto MapToDto(CompanyHR h) => new()
        {
            CompanyHRId = h.CompanyHRId,
            UserId = h.UserId,
            FullName = h.User?.FullName ?? string.Empty,
            Email = h.User?.Email ?? string.Empty,
            PhoneNumber = h.User?.PhoneNumber ?? string.Empty,
            CompanyId = h.CompanyId,
            CompanyName = h.Company?.CompanyName ?? string.Empty,
            Designation = h.Designation,
            Status = h.User?.AccountStatus.ToString() ?? AccountStatus.Pending.ToString(),
            ApprovedBy = h.ApprovedBy,
            ApprovedAt = h.ApprovedAt
        };
    }
}