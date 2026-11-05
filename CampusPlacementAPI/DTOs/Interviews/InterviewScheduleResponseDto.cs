using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.DTOs.Interviews
{
    public class InterviewScheduleResponseDto
    {
        public int InterviewScheduleId { get; set; }
        public int ApplicationId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public int InterviewRoundId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public int RoundNumber { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string? MeetingLink { get; set; }
        public string? Venue { get; set; }
        public InterviewStatus Status { get; set; }
        public string? Feedback { get; set; }
    }
}