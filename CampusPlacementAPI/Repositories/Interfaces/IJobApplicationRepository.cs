using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task<JobApplication?> GetByIdAsync(int applicationId);
        Task<IEnumerable<JobApplication>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<JobApplication>> GetByJobIdAsync(int jobId);
        Task<JobApplication?> GetByStudentAndJobAsync(int studentId, int jobId);
        Task AddAsync(JobApplication application);
        Task UpdateAsync(JobApplication application);
        Task AddStatusHistoryAsync(ApplicationStatusHistory history);
        Task<IEnumerable<ApplicationStatusHistory>> GetStatusHistoriesAsync(int applicationId);
    }
}