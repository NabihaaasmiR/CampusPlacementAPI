using CampusPlacementAPI.DTOs.PlacementOfficers;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface IPlacementOfficerService
    {
        Task<IEnumerable<PlacementOfficerResponseDto>> GetAllOfficersAsync();
        Task<PlacementOfficerResponseDto?> GetOfficerByIdAsync(int id);
        Task<PlacementOfficerResponseDto?> GetOfficerByUserIdAsync(string userId);
        Task<IEnumerable<PlacementOfficerResponseDto>> GetPendingOfficersAsync();
        Task<PlacementOfficerResponseDto> RegisterOfficerAsync(PlacementOfficerRegisterDto registerDto);
        Task<bool> ApproveOfficerAsync(int id, string adminUserId);
        Task<bool> RejectOfficerAsync(int id);
        Task<bool> SuspendOfficerAsync(int id);
    }
}