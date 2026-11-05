using CampusPlacementAPI.Models;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}