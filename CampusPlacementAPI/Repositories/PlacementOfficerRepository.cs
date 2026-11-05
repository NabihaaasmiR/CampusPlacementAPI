using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class PlacementOfficerRepository : IPlacementOfficerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlacementOfficerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlacementOfficer>> GetAllAsync()
        {
            return await _context.PlacementOfficers
                .Include(po => po.User)
                .Include(po => po.College)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PlacementOfficer?> GetByIdAsync(int placementOfficerId)
        {
            return await _context.PlacementOfficers
                .Include(po => po.User)
                .Include(po => po.College)
                .FirstOrDefaultAsync(po => po.PlacementOfficerId == placementOfficerId);
        }

        public async Task<PlacementOfficer?> GetByUserIdAsync(string userId)
        {
            return await _context.PlacementOfficers
                .Include(po => po.User)
                .Include(po => po.College)
                .FirstOrDefaultAsync(po => po.UserId == userId);
        }

        public async Task<IEnumerable<PlacementOfficer>> GetByStatusAsync(AccountStatus status)
        {
            return await _context.PlacementOfficers
                .Include(po => po.User)
                .Include(po => po.College)
                .Where(po => po.Status == status)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<PlacementOfficer>> GetByCollegeIdAsync(int collegeId)
        {
            return await _context.PlacementOfficers
                .Include(po => po.User)
                .Include(po => po.College)
                .Where(po => po.CollegeId == collegeId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PlacementOfficer> AddAsync(PlacementOfficer officer)
        {
            await _context.PlacementOfficers.AddAsync(officer);
            await _context.SaveChangesAsync();
            return officer;
        }

        public async Task UpdateAsync(PlacementOfficer officer)
        {
            _context.PlacementOfficers.Update(officer);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int placementOfficerId)
        {
            return await _context.PlacementOfficers.AnyAsync(po => po.PlacementOfficerId == placementOfficerId);
        }
    }
}