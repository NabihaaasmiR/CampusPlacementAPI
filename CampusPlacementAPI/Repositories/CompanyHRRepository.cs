using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class CompanyHRRepository : ICompanyHRRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyHRRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<CompanyHR>> GetAllAsync() =>
            await _context.CompanyHRs.Include(h => h.User).Include(h => h.Company).AsNoTracking().ToListAsync();

        public async Task<CompanyHR?> GetByIdAsync(int companyHRId) =>
            await _context.CompanyHRs.Include(h => h.User).Include(h => h.Company).FirstOrDefaultAsync(h => h.CompanyHRId == companyHRId);

        public async Task<CompanyHR?> GetByUserIdAsync(string userId) =>
            await _context.CompanyHRs.Include(h => h.User).Include(h => h.Company).FirstOrDefaultAsync(h => h.UserId == userId);

        public async Task<IEnumerable<CompanyHR>> GetByStatusAsync(AccountStatus status) =>
            await _context.CompanyHRs.Include(h => h.User).Include(h => h.Company).Where(h => h.User.AccountStatus == status).AsNoTracking().ToListAsync();

        public async Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(int companyId) =>
            await _context.CompanyHRs.Include(h => h.User).Include(h => h.Company).Where(h => h.CompanyId == companyId).AsNoTracking().ToListAsync();

        public async Task<CompanyHR> AddAsync(CompanyHR companyHR)
        {
            await _context.CompanyHRs.AddAsync(companyHR);
            await _context.SaveChangesAsync();
            return companyHR;
        }

        public async Task UpdateAsync(CompanyHR companyHR)
        {
            _context.CompanyHRs.Update(companyHR);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int companyHRId) =>
            await _context.CompanyHRs.AnyAsync(h => h.CompanyHRId == companyHRId);
    }
}