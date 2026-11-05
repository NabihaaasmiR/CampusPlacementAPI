using CampusPlacementAPI.DTOs.JobApplications;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;

namespace CampusPlacementAPI.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _applicationRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IJobOpeningRepository _jobRepo; // Assuming job repo exists or accessed via context/repo

        public JobApplicationService(
            IJobApplicationRepository applicationRepo,
            IStudentRepository studentRepo,
            IJobOpeningRepository jobRepo)
        {
            _applicationRepo = applicationRepo;
            _studentRepo = studentRepo;
            _jobRepo = jobRepo;
        }

        public async Task<JobApplicationResponseDto> ApplyForJobAsync(string userId, JobApplicationCreateDto dto)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId);
            if (student == null)
                throw new KeyNotFoundException("Student profile not found for current user.");

            if (student.Status != AccountStatus.Approved)
                throw new InvalidOperationException("Your student account must be approved before applying for jobs.");

            var job = await _jobRepo.GetByIdAsync(dto.JobId);
            if (job == null || job.Status != JobStatus.Published)
                throw new InvalidOperationException("Job opening not found or is not currently published.");

            if (DateTime.UtcNow > job.ApplicationDeadline)
                throw new InvalidOperationException("The application deadline for this job has passed.");

            // Check eligibility (CGPA & Backlogs)
            if (job.MinimumCGPA.HasValue && student.CGPA < job.MinimumCGPA.Value)
                throw new InvalidOperationException($"Your CGPA ({student.CGPA}) does not meet the minimum requirement ({job.MinimumCGPA.Value}) for this job.");

            if (job.MaximumBacklogs.HasValue && student.ActiveBacklogs > job.MaximumBacklogs.Value)
                throw new InvalidOperationException($"Your active backlogs ({student.ActiveBacklogs}) exceed the maximum allowed ({job.MaximumBacklogs.Value}) for this job.");

            var existingApp = await _applicationRepo.GetByStudentAndJobAsync(student.StudentId, dto.JobId);
            if (existingApp != null)
                throw new InvalidOperationException("You have already applied for this job opening.");

            var application = new JobApplication
            {
                JobId = dto.JobId,
                StudentId = student.StudentId,
                AppliedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Applied,
                ResumeUrl = dto.ResumeUrl ?? student.ResumeUrl,
                LastUpdatedAt = DateTime.UtcNow
            };

            await _applicationRepo.AddAsync(application);

            // Record initial status history
            var history = new ApplicationStatusHistory
            {
                ApplicationId = application.ApplicationId,
                PreviousStatus = null,
                NewStatus = ApplicationStatus.Applied,
                UpdatedByUserId = userId,
                Remarks = "Student submitted job application.",
                UpdatedAt = DateTime.UtcNow
            };
            await _applicationRepo.AddStatusHistoryAsync(history);

            return MapToDto(application);
        }

        public async Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsForStudentAsync(string userId)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId);
            if (student == null)
                throw new KeyNotFoundException("Student profile not found.");

            var apps = await _applicationRepo.GetByStudentIdAsync(student.StudentId);
            return apps.Select(MapToDto);
        }

        public async Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsForJobAsync(int jobId)
        {
            var apps = await _applicationRepo.GetByJobIdAsync(jobId);
            return apps.Select(MapToDto);
        }

        public async Task<JobApplicationResponseDto> UpdateApplicationStatusAsync(int applicationId, string updatedByUserId, JobApplicationStatusUpdateDto dto)
        {
            var app = await _applicationRepo.GetByIdAsync(applicationId);
            if (app == null)
                throw new KeyNotFoundException("Job application not found.");

            var prevStatus = app.Status;
            app.Status = dto.NewStatus;
            app.LastUpdatedAt = DateTime.UtcNow;

            await _applicationRepo.UpdateAsync(app);

            var history = new ApplicationStatusHistory
            {
                ApplicationId = app.ApplicationId,
                PreviousStatus = prevStatus,
                NewStatus = dto.NewStatus,
                UpdatedByUserId = updatedByUserId,
                Remarks = dto.Remarks,
                UpdatedAt = DateTime.UtcNow
            };
            await _applicationRepo.AddStatusHistoryAsync(history);

            return MapToDto(app);
        }

        private static JobApplicationResponseDto MapToDto(JobApplication app) => new()
        {
            ApplicationId = app.ApplicationId,
            JobId = app.JobId,
            JobTitle = app.JobOpening?.JobTitle ?? string.Empty,
            StudentId = app.StudentId,
            StudentName = app.Student?.User?.FullName ?? string.Empty,
            RegisterNumber = app.Student?.RegisterNumber ?? string.Empty,
            AppliedAt = app.AppliedAt,
            Status = app.Status,
            ResumeUrl = app.ResumeUrl,
            LastUpdatedAt = app.LastUpdatedAt
        };
    }
}