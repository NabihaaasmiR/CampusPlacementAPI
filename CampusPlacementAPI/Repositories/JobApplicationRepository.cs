using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobApplication?> GetByIdAsync(int applicationId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobOpening)
                .Include(ja => ja.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(ja => ja.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<JobApplication>> GetByStudentIdAsync(int studentId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobOpening)
                .Include(ja => ja.Student)
                .ThenInclude(s => s.User)
                .Where(ja => ja.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetByJobIdAsync(int jobId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobOpening)
                .Include(ja => ja.Student)
                .ThenInclude(s => s.User)
                .Where(ja => ja.JobId == jobId)
                .ToListAsync();
        }

        public async Task<JobApplication?> GetByStudentAndJobAsync(int studentId, int jobId)
        {
            return await _context.JobApplications
                .FirstOrDefaultAsync(ja => ja.StudentId == studentId && ja.JobId == jobId);
        }

        public async Task AddAsync(JobApplication application)
        {
            await _context.JobApplications.AddAsync(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobApplication application)
        {
            _context.JobApplications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task AddStatusHistoryAsync(ApplicationStatusHistory history)
        {
            await _context.ApplicationStatusHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationStatusHistory>> GetStatusHistoriesAsync(int applicationId)
        {
            return await _context.ApplicationStatusHistories
                .Where(ash => ash.ApplicationId == applicationId)
                .OrderByDescending(ash => ash.UpdatedAt)
                .ToListAsync();
        }
    }
}