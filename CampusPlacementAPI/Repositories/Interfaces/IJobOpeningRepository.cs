using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface IJobOpeningRepository
    {
        Task<IEnumerable<JobOpening>> GetAllAsync();
        Task<JobOpening?> GetByIdAsync(int jobId);
        Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId);
        Task<IEnumerable<JobOpening>> GetPublishedAsync();
        Task<JobOpening> AddAsync(JobOpening jobOpening);
        Task UpdateAsync(JobOpening jobOpening);
        Task DeleteAsync(JobOpening jobOpening);
        Task<bool> ExistsAsync(int jobId);
    }
}