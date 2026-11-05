using CampusPlacementAPI.DTOs.JobOpenings;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;

namespace CampusPlacementAPI.Services
{
    public class JobOpeningService : IJobOpeningService
    {
        private readonly IJobOpeningRepository _jobRepository;
        private readonly ICompanyHRRepository _hrRepository;

        public JobOpeningService(IJobOpeningRepository jobRepository, ICompanyHRRepository hrRepository)
        {
            _jobRepository = jobRepository;
            _hrRepository = hrRepository;
        }

        public async Task<IEnumerable<JobOpeningResponseDto>> GetAllJobsAsync() =>
            (await _jobRepository.GetAllAsync()).Select(MapToDto);

        public async Task<JobOpeningResponseDto?> GetJobByIdAsync(int jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            return job == null ? null : MapToDto(job);
        }

        public async Task<IEnumerable<JobOpeningResponseDto>> GetJobsByCompanyAsync(int companyId) =>
            (await _jobRepository.GetByCompanyIdAsync(companyId)).Select(MapToDto);

        public async Task<IEnumerable<JobOpeningResponseDto>> GetPublishedJobsAsync() =>
            (await _jobRepository.GetPublishedAsync()).Select(MapToDto);

        public async Task<JobOpeningResponseDto> CreateJobAsync(int companyId, int hrId, JobOpeningCreateDto dto)
        {
            var job = new JobOpening
            {
                CompanyId = companyId,
                CreatedByHRId = hrId,
                JobTitle = dto.JobTitle,
                JobDescription = dto.JobDescription,
                JobType = dto.JobType,
                Location = dto.Location,
                SalaryPackage = dto.SalaryPackage,
                MinimumCGPA = dto.MinimumCGPA,
                MaximumBacklogs = dto.MaximumBacklogs,
                ApplicationDeadline = dto.ApplicationDeadline,
                IsOpenToAllColleges = dto.IsOpenToAllColleges,
                Status = JobStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            if (!dto.IsOpenToAllColleges && dto.TargetCollegeIds != null)
            {
                job.JobTargetColleges = dto.TargetCollegeIds.Select(collegeId => new JobTargetCollege
                {
                    CollegeId = collegeId
                }).ToList();
            }

            var created = await _jobRepository.AddAsync(job);
            return MapToDto(created);
        }

        public async Task<bool> PublishJobAsync(int jobId, int companyId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null || job.CompanyId != companyId) return false;

            job.Status = JobStatus.Published;
            await _jobRepository.UpdateAsync(job);
            return true;
        }

        public async Task<bool> CloseJobAsync(int jobId, int companyId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null || job.CompanyId != companyId) return false;

            job.Status = JobStatus.Closed;
            await _jobRepository.UpdateAsync(job);
            return true;
        }

        private static JobOpeningResponseDto MapToDto(JobOpening j) => new()
        {
            JobId = j.JobId,
            CompanyId = j.CompanyId,
            CompanyName = j.Company?.CompanyName ?? string.Empty,
            JobTitle = j.JobTitle,
            JobDescription = j.JobDescription,
            JobType = j.JobType,
            Location = j.Location,
            SalaryPackage = j.SalaryPackage,
            MinimumCGPA = j.MinimumCGPA,
            MaximumBacklogs = j.MaximumBacklogs,
            ApplicationDeadline = j.ApplicationDeadline,
            IsOpenToAllColleges = j.IsOpenToAllColleges,
            Status = j.Status.ToString(),
            CreatedAt = j.CreatedAt,
            TargetColleges = j.JobTargetColleges?.Select(jt => jt.College?.CollegeName ?? string.Empty).ToList() ?? new List<string>()
        };
    }
}