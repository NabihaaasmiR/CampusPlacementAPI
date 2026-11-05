using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly ApplicationDbContext _context;

        public CollegeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<College>> GetAllAsync()
        {
            return await _context.Colleges
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<College?> GetByIdAsync(int collegeId)
        {
            return await _context.Colleges
                .Include(c => c.PlacementOfficers)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.CollegeId == collegeId);
        }

        public async Task<College?> GetByCodeAsync(string collegeCode)
        {
            return await _context.Colleges
                .FirstOrDefaultAsync(c => c.CollegeCode.ToLower() == collegeCode.ToLower());
        }

        public async Task<College?> GetByEmailAsync(string officialEmail)
        {
            return await _context.Colleges
                .FirstOrDefaultAsync(c => c.OfficialEmail.ToLower() == officialEmail.ToLower());
        }

        public async Task<IEnumerable<College>> GetByStatusAsync(CollegeStatus status)
        {
            return await _context.Colleges
                .Where(c => c.Status == status)
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<College> AddAsync(College college)
        {
            await _context.Colleges.AddAsync(college);
            await _context.SaveChangesAsync();
            return college;
        }

        public async Task UpdateAsync(College college)
        {
            _context.Colleges.Update(college);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int collegeId)
        {
            return await _context.Colleges.AnyAsync(c => c.CollegeId == collegeId);
        }
    }
}