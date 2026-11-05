using CampusPlacementAPI.DTOs.Auth;

namespace CampusPlacementAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}