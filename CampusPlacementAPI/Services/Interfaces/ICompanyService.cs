using CampusPlacementAPI.DTOs.Companies;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync();
        Task<CompanyResponseDto?> GetCompanyByIdAsync(int id);
        Task<IEnumerable<CompanyResponseDto>> GetPendingCompaniesAsync();
        Task<CompanyResponseDto> RegisterCompanyAsync(CompanyRegisterDto dto);
        Task<bool> ApproveCompanyAsync(int id);
        Task<bool> RejectCompanyAsync(int id);
        Task<bool> SuspendCompanyAsync(int id);
        Task<bool> UpdateCompanyAsync(int id, CompanyUpdateDto dto);
    }
}