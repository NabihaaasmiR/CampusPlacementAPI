using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface IInterviewRepository
    {
        Task AddRoundAsync(InterviewRound round);
        Task<InterviewRound?> GetRoundByIdAsync(int roundId);
        Task<IEnumerable<InterviewRound>> GetRoundsByJobIdAsync(int jobId);

        Task AddScheduleAsync(InterviewSchedule schedule);
        Task<InterviewSchedule?> GetScheduleByIdAsync(int scheduleId);
        Task<IEnumerable<InterviewSchedule>> GetSchedulesByApplicationIdAsync(int applicationId);
        Task<IEnumerable<InterviewSchedule>> GetSchedulesByStudentIdAsync(int studentId);
        Task UpdateScheduleAsync(InterviewSchedule schedule);
    }
}