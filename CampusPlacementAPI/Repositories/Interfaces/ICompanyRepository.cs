using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company?> GetByIdAsync(int companyId);
        Task<Company?> GetByEmailAsync(string companyEmail);
        Task<IEnumerable<Company>> GetByStatusAsync(CollegeStatus status);
        Task<Company> AddAsync(Company company);
        Task UpdateAsync(Company company);
        Task<bool> ExistsAsync(int companyId);
    }
}