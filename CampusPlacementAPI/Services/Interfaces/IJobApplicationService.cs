using CampusPlacementAPI.DTOs.JobApplications;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface IJobApplicationService
    {
        Task<JobApplicationResponseDto> ApplyForJobAsync(string userId, JobApplicationCreateDto dto);
        Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsForStudentAsync(string userId);
        Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsForJobAsync(int jobId);
        Task<JobApplicationResponseDto> UpdateApplicationStatusAsync(int applicationId, string updatedByUserId, JobApplicationStatusUpdateDto dto);
    }
}