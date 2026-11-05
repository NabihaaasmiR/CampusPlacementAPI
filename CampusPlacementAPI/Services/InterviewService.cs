using CampusPlacementAPI.DTOs.Interviews;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using CampusPlacementAPI.Services.Interfaces;

namespace CampusPlacementAPI.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepo;
        private readonly IJobApplicationRepository _applicationRepo;
        private readonly IStudentRepository _studentRepo;

        public InterviewService(
            IInterviewRepository interviewRepo,
            IJobApplicationRepository applicationRepo,
            IStudentRepository studentRepo)
        {
            _interviewRepo = interviewRepo;
            _applicationRepo = applicationRepo;
            _studentRepo = studentRepo;
        }

        public async Task<InterviewRound> CreateRoundAsync(InterviewRoundCreateDto dto)
        {
            var round = new InterviewRound
            {
                JobId = dto.JobId,
                RoundName = dto.RoundName,
                RoundNumber = dto.RoundNumber,
                Description = dto.Description
            };

            await _interviewRepo.AddRoundAsync(round);
            return round;
        }

        public async Task<IEnumerable<InterviewRound>> GetRoundsForJobAsync(int jobId)
        {
            return await _interviewRepo.GetRoundsByJobIdAsync(jobId);
        }

        public async Task<InterviewScheduleResponseDto> ScheduleInterviewAsync(InterviewScheduleCreateDto dto)
        {
            var app = await _applicationRepo.GetByIdAsync(dto.ApplicationId);
            if (app == null)
                throw new KeyNotFoundException("Job application not found.");

            var round = await _interviewRepo.GetRoundByIdAsync(dto.InterviewRoundId);
            if (round == null || round.JobId != app.JobId)
                throw new InvalidOperationException("Invalid interview round for this job opening.");

            var schedule = new InterviewSchedule
            {
                ApplicationId = dto.ApplicationId,
                InterviewRoundId = dto.InterviewRoundId,
                ScheduledAt = dto.ScheduledAt,
                MeetingLink = dto.MeetingLink,
                Venue = dto.Venue,
                Status = InterviewStatus.Scheduled
            };

            await _interviewRepo.AddScheduleAsync(schedule);

            var created = await _interviewRepo.GetScheduleByIdAsync(schedule.InterviewScheduleId);
            return MapToDto(created!);
        }

        public async Task<InterviewScheduleResponseDto> UpdateInterviewScheduleAsync(int scheduleId, InterviewScheduleUpdateDto dto)
        {
            var schedule = await _interviewRepo.GetScheduleByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException("Interview schedule not found.");

            schedule.ScheduledAt = dto.ScheduledAt;
            schedule.MeetingLink = dto.MeetingLink;
            schedule.Venue = dto.Venue;
            schedule.Status = dto.Status;
            schedule.Feedback = dto.Feedback;

            await _interviewRepo.UpdateScheduleAsync(schedule);
            return MapToDto(schedule);
        }

        public async Task<IEnumerable<InterviewScheduleResponseDto>> GetSchedulesForStudentAsync(string userId)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId);
            if (student == null)
                throw new KeyNotFoundException("Student profile not found.");

            var schedules = await _interviewRepo.GetSchedulesByStudentIdAsync(student.StudentId);
            return schedules.Select(MapToDto);
        }

        private static InterviewScheduleResponseDto MapToDto(InterviewSchedule s) => new()
        {
            InterviewScheduleId = s.InterviewScheduleId,
            ApplicationId = s.ApplicationId,
            StudentName = s.JobApplication?.Student?.User?.FullName ?? string.Empty,
            JobTitle = s.JobApplication?.JobOpening?.JobTitle ?? string.Empty,
            InterviewRoundId = s.InterviewRoundId,
            RoundName = s.InterviewRound?.RoundName ?? string.Empty,
            RoundNumber = s.InterviewRound?.RoundNumber ?? 0,
            ScheduledAt = s.ScheduledAt,
            MeetingLink = s.MeetingLink,
            Venue = s.Venue,
            Status = s.Status,
            Feedback = s.Feedback
        };
    }
}