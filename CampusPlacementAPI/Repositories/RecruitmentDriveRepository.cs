using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class RecruitmentDriveRepository : IRecruitmentDriveRepository
    {
        private readonly ApplicationDbContext _context;

        public RecruitmentDriveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RecruitmentDrive?> GetByIdAsync(int driveId)
        {
            return await _context.RecruitmentDrives
                .Include(rd => rd.JobOpening)
                .ThenInclude(j => j.Company)
                .Include(rd => rd.College)
                .FirstOrDefaultAsync(rd => rd.DriveId == driveId);
        }

        public async Task<IEnumerable<RecruitmentDrive>> GetByJobIdAsync(int jobId)
        {
            return await _context.RecruitmentDrives
                .Include(rd => rd.JobOpening)
                .ThenInclude(j => j.Company)
                .Include(rd => rd.College)
                .Where(rd => rd.JobId == jobId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RecruitmentDrive>> GetByCollegeIdAsync(int collegeId)
        {
            return await _context.RecruitmentDrives
                .Include(rd => rd.JobOpening)
                .ThenInclude(j => j.Company)
                .Include(rd => rd.College)
                .Where(rd => rd.CollegeId == collegeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RecruitmentDrive>> GetAllAsync()
        {
            return await _context.RecruitmentDrives
                .Include(rd => rd.JobOpening)
                .ThenInclude(j => j.Company)
                .Include(rd => rd.College)
                .ToListAsync();
        }

        public async Task AddAsync(RecruitmentDrive drive)
        {
            await _context.RecruitmentDrives.AddAsync(drive);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecruitmentDrive drive)
        {
            _context.RecruitmentDrives.Update(drive);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RecruitmentDrive drive)
        {
            _context.RecruitmentDrives.Remove(drive);
            await _context.SaveChangesAsync();
        }
    }
}