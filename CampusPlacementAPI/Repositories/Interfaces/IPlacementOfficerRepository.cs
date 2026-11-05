using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Repositories.Interfaces
{
    public interface IPlacementOfficerRepository
    {
        Task<IEnumerable<PlacementOfficer>> GetAllAsync();
        Task<PlacementOfficer?> GetByIdAsync(int placementOfficerId);
        Task<PlacementOfficer?> GetByUserIdAsync(string userId);
        Task<IEnumerable<PlacementOfficer>> GetByStatusAsync(AccountStatus status);
        Task<IEnumerable<PlacementOfficer>> GetByCollegeIdAsync(int collegeId);
        Task<PlacementOfficer> AddAsync(PlacementOfficer officer);
        Task UpdateAsync(PlacementOfficer officer);
        Task<bool> ExistsAsync(int placementOfficerId);
    }
}