using CampusPlacementAPI.DTOs.RecruitmentDrives;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface IRecruitmentDriveService
    {
        Task<RecruitmentDriveResponseDto> CreateDriveAsync(RecruitmentDriveCreateDto dto);
        Task<RecruitmentDriveResponseDto> UpdateDriveAsync(int driveId, RecruitmentDriveUpdateDto dto);
        Task<RecruitmentDriveResponseDto?> GetDriveByIdAsync(int driveId);
        Task<IEnumerable<RecruitmentDriveResponseDto>> GetDrivesForJobAsync(int jobId);
        Task<IEnumerable<RecruitmentDriveResponseDto>> GetDrivesForCollegeAsync(int collegeId);
        Task<IEnumerable<RecruitmentDriveResponseDto>> GetAllDrivesAsync();
        Task DeleteDriveAsync(int driveId);
    }
}