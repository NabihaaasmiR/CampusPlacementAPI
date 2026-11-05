using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface IRecruitmentDriveRepository
    {
        Task<RecruitmentDrive?> GetByIdAsync(int driveId);
        Task<IEnumerable<RecruitmentDrive>> GetByJobIdAsync(int jobId);
        Task<IEnumerable<RecruitmentDrive>> GetByCollegeIdAsync(int collegeId);
        Task<IEnumerable<RecruitmentDrive>> GetAllAsync();
        Task AddAsync(RecruitmentDrive drive);
        Task UpdateAsync(RecruitmentDrive drive);
        Task DeleteAsync(RecruitmentDrive drive);
    }
}