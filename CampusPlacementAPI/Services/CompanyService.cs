using CampusPlacementAPI.DTOs.Companies;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;

namespace CampusPlacementAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository) => _companyRepository = companyRepository;

        public async Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync() =>
            (await _companyRepository.GetAllAsync()).Select(MapToDto);

        public async Task<CompanyResponseDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            return company == null ? null : MapToDto(company);
        }

        public async Task<IEnumerable<CompanyResponseDto>> GetPendingCompaniesAsync() =>
            (await _companyRepository.GetByStatusAsync(CollegeStatus.Pending)).Select(MapToDto);

        public async Task<CompanyResponseDto> RegisterCompanyAsync(CompanyRegisterDto dto)
        {
            var existing = await _companyRepository.GetByEmailAsync(dto.CompanyEmail);
            if (existing != null) throw new InvalidOperationException("A company with this email already exists.");

            var company = new Company
            {
                CompanyName = dto.CompanyName,
                CompanyEmail = dto.CompanyEmail,
                Website = dto.Website,
                Industry = dto.Industry,
                Address = dto.Address,
                Description = dto.Description,
                Status = CollegeStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _companyRepository.AddAsync(company);
            return MapToDto(created);
        }

        public async Task<bool> ApproveCompanyAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return false;
            company.Status = CollegeStatus.Approved;
            await _companyRepository.UpdateAsync(company);
            return true;
        }

        public async Task<bool> RejectCompanyAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return false;
            company.Status = CollegeStatus.Rejected;
            await _companyRepository.UpdateAsync(company);
            return true;
        }

        public async Task<bool> SuspendCompanyAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return false;
            company.Status = CollegeStatus.Suspended;
            await _companyRepository.UpdateAsync(company);
            return true;
        }

        public async Task<bool> UpdateCompanyAsync(int id, CompanyUpdateDto dto)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return false;

            company.CompanyName = dto.CompanyName;
            company.CompanyEmail = dto.CompanyEmail;
            company.Website = dto.Website;
            company.Industry = dto.Industry;
            company.Address = dto.Address;
            company.Description = dto.Description;
            company.Status = dto.Status;

            await _companyRepository.UpdateAsync(company);
            return true;
        }

        private static CompanyResponseDto MapToDto(Company c) => new()
        {
            CompanyId = c.CompanyId,
            CompanyName = c.CompanyName,
            CompanyEmail = c.CompanyEmail,
            Website = c.Website,
            Industry = c.Industry,
            Address = c.Address,
            Description = c.Description,
            Status = c.Status.ToString(),
            CreatedAt = c.CreatedAt
        };
    }
}