using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementAPI.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Company>> GetAllAsync() =>
            await _context.Companies.AsNoTracking().OrderByDescending(c => c.CreatedAt).ToListAsync();

        public async Task<Company?> GetByIdAsync(int companyId) =>
            await _context.Companies
                .Include(c => c.CompanyHRs)
                .Include(c => c.JobOpenings)
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);

        public async Task<Company?> GetByEmailAsync(string companyEmail) =>
            await _context.Companies.FirstOrDefaultAsync(c => c.CompanyEmail.ToLower() == companyEmail.ToLower());

        public async Task<IEnumerable<Company>> GetByStatusAsync(CollegeStatus status) =>
            await _context.Companies.Where(c => c.Status == status).AsNoTracking().ToListAsync();

        public async Task<Company> AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int companyId) =>
            await _context.Companies.AnyAsync(c => c.CompanyId == companyId);
    }
}