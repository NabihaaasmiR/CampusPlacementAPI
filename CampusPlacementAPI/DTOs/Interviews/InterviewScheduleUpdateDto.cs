using System.ComponentModel.DataAnnotations;
using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.DTOs.Interviews
{
    public class InterviewScheduleUpdateDto
    {
        [Required]
        public DateTime ScheduledAt { get; set; }

        [MaxLength(500)]
        public string? MeetingLink { get; set; }

        [MaxLength(255)]
        public string? Venue { get; set; }

        [Required]
        public InterviewStatus Status { get; set; }

        public string? Feedback { get; set; }
    }
}