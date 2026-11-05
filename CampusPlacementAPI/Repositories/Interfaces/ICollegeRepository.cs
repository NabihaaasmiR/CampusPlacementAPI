using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface ICollegeRepository
    {
        Task<IEnumerable<College>> GetAllAsync();
        Task<College?> GetByIdAsync(int collegeId);
        Task<College?> GetByCodeAsync(string collegeCode);
        Task<College?> GetByEmailAsync(string officialEmail);
        Task<IEnumerable<College>> GetByStatusAsync(CollegeStatus status);
        Task<College> AddAsync(College college);
        Task UpdateAsync(College college);
        Task<bool> ExistsAsync(int collegeId);
    }
}