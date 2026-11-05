using CampusPlacementAPI.DTOs.Interviews;
using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface IInterviewService
    {
        Task<InterviewRound> CreateRoundAsync(InterviewRoundCreateDto dto);
        Task<IEnumerable<InterviewRound>> GetRoundsForJobAsync(int jobId);
        Task<InterviewScheduleResponseDto> ScheduleInterviewAsync(InterviewScheduleCreateDto dto);
        Task<InterviewScheduleResponseDto> UpdateInterviewScheduleAsync(int scheduleId, InterviewScheduleUpdateDto dto);
        Task<IEnumerable<InterviewScheduleResponseDto>> GetSchedulesForStudentAsync(string userId);
    }
}