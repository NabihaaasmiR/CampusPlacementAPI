using CampusPlacementAPI.DTOs.JobOpenings;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface IJobOpeningService
    {
        Task<IEnumerable<JobOpeningResponseDto>> GetAllJobsAsync();
        Task<JobOpeningResponseDto?> GetJobByIdAsync(int jobId);
        Task<IEnumerable<JobOpeningResponseDto>> GetJobsByCompanyAsync(int companyId);
        Task<IEnumerable<JobOpeningResponseDto>> GetPublishedJobsAsync();
        Task<JobOpeningResponseDto> CreateJobAsync(int companyId, int hrId, JobOpeningCreateDto dto);
        Task<bool> PublishJobAsync(int jobId, int companyId);
        Task<bool> CloseJobAsync(int jobId, int companyId);
    }
}