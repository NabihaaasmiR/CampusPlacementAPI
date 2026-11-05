using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface ICompanyHRRepository
    {
        Task<IEnumerable<CompanyHR>> GetAllAsync();
        Task<CompanyHR?> GetByIdAsync(int companyHRId);
        Task<CompanyHR?> GetByUserIdAsync(string userId);
        Task<IEnumerable<CompanyHR>> GetByStatusAsync(AccountStatus status);
        Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(int companyId);
        Task<CompanyHR> AddAsync(CompanyHR companyHR);
        Task UpdateAsync(CompanyHR companyHR);
        Task<bool> ExistsAsync(int companyHRId);
    }
}