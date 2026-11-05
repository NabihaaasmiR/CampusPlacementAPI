using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class JobOpeningRepository : IJobOpeningRepository
    {
        private readonly ApplicationDbContext _context;

        public JobOpeningRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<JobOpening>> GetAllAsync() =>
            await _context.JobOpenings
                .Include(j => j.Company)
                .Include(j => j.JobTargetColleges)
                .ThenInclude(jt => jt.College)
                .AsNoTracking()
                .ToListAsync();

        public async Task<JobOpening?> GetByIdAsync(int jobId) =>
            await _context.JobOpenings
                .Include(j => j.Company)
                .Include(j => j.JobTargetColleges)
                .ThenInclude(jt => jt.College)
                .FirstOrDefaultAsync(j => j.JobId == jobId);

        public async Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId) =>
            await _context.JobOpenings
                .Include(j => j.Company)
                .Include(j => j.JobTargetColleges)
                .ThenInclude(jt => jt.College)
                .Where(j => j.CompanyId == companyId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<JobOpening>> GetPublishedAsync() =>
            await _context.JobOpenings
                .Include(j => j.Company)
                .Include(j => j.JobTargetColleges)
                .ThenInclude(jt => jt.College)
                .Where(j => j.Status == JobStatus.Published && j.ApplicationDeadline > DateTime.UtcNow)
                .AsNoTracking()
                .ToListAsync();

        public async Task<JobOpening> AddAsync(JobOpening jobOpening)
        {
            await _context.JobOpenings.AddAsync(jobOpening);
            await _context.SaveChangesAsync();
            return jobOpening;
        }

        public async Task UpdateAsync(JobOpening jobOpening)
        {
            _context.JobOpenings.Update(jobOpening);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(JobOpening jobOpening)
        {
            _context.JobOpenings.Remove(jobOpening);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int jobId) =>
            await _context.JobOpenings.AnyAsync(j => j.JobId == jobId);
    }
}