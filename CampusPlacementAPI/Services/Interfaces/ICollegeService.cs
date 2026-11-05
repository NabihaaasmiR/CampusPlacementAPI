using CampusPlacementAPI.DTOs.Colleges;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface ICollegeService
    {
        Task<IEnumerable<CollegeResponseDto>> GetAllCollegesAsync();
        Task<CollegeResponseDto?> GetCollegeByIdAsync(int collegeId);
        Task<IEnumerable<CollegeResponseDto>> GetPendingCollegesAsync();
        Task<CollegeResponseDto> RegisterCollegeAsync(CollegeRegisterDto registerDto);
        Task<bool> ApproveCollegeAsync(int collegeId);
        Task<bool> RejectCollegeAsync(int collegeId);
        Task<bool> SuspendCollegeAsync(int collegeId);
        Task<bool> UpdateCollegeAsync(int collegeId, CollegeUpdateDto updateDto);
    }
}