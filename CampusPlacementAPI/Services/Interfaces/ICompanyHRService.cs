using CampusPlacementAPI.DTOs.CompanyHRs;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface ICompanyHRService
    {
        Task<IEnumerable<CompanyHRResponseDto>> GetAllHRsAsync();
        Task<CompanyHRResponseDto?> GetHRByIdAsync(int id);
        Task<CompanyHRResponseDto?> GetHRByUserIdAsync(string userId);
        Task<IEnumerable<CompanyHRResponseDto>> GetPendingHRsAsync();
        Task<CompanyHRResponseDto> RegisterHRAsync(CompanyHRRegisterDto dto);
        Task<bool> ApproveHRAsync(int id, string adminUserId);
        Task<bool> RejectHRAsync(int id);
    }
}