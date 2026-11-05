using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRoundAsync(InterviewRound round)
        {
            await _context.Set<InterviewRound>().AddAsync(round);
            await _context.SaveChangesAsync();
        }

        public async Task<InterviewRound?> GetRoundByIdAsync(int roundId)
        {
            return await _context.Set<InterviewRound>()
                .Include(ir => ir.JobOpening)
                .FirstOrDefaultAsync(ir => ir.InterviewRoundId == roundId);
        }

        public async Task<IEnumerable<InterviewRound>> GetRoundsByJobIdAsync(int jobId)
        {
            return await _context.Set<InterviewRound>()
                .Where(ir => ir.JobId == jobId)
                .OrderBy(ir => ir.RoundNumber)
                .ToListAsync();
        }

        public async Task AddScheduleAsync(InterviewSchedule schedule)
        {
            await _context.InterviewSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<InterviewSchedule?> GetScheduleByIdAsync(int scheduleId)
        {
            return await _context.InterviewSchedules
                .Include(isched => isched.JobApplication)
                .ThenInclude(ja => ja.Student)
                .ThenInclude(s => s.User)
                .Include(isched => isched.JobApplication)
                .ThenInclude(ja => ja.JobOpening)
                .Include(isched => isched.InterviewRound)
                .FirstOrDefaultAsync(isched => isched.InterviewScheduleId == scheduleId);
        }

        public async Task<IEnumerable<InterviewSchedule>> GetSchedulesByApplicationIdAsync(int applicationId)
        {
            return await _context.InterviewSchedules
                .Include(isched => isched.JobApplication)
                .ThenInclude(ja => ja.Student)
                .ThenInclude(s => s.User)
                .Include(isched => isched.InterviewRound)
                .Where(isched => isched.ApplicationId == applicationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InterviewSchedule>> GetSchedulesByStudentIdAsync(int studentId)
        {
            return await _context.InterviewSchedules
                .Include(isched => isched.JobApplication)
                .ThenInclude(ja => ja.JobOpening)
                .Include(isched => isched.InterviewRound)
                .Where(isched => isched.JobApplication.StudentId == studentId)
                .ToListAsync();
        }

        public async Task UpdateScheduleAsync(InterviewSchedule schedule)
        {
            _context.InterviewSchedules.Update(schedule);
            await _context.SaveChangesAsync();
        }
    }
}