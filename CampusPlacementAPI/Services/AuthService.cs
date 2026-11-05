using CampusPlacementAPI.DTOs.Auth;
using CampusPlacementAPI.Models;
using CampusPlacementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Enforce TDD Account Activation Rules
            if (user.AccountStatus != AccountStatus.Approved)
            {
                throw new UnauthorizedAccessException($"Login denied. Account status is '{user.AccountStatus}'. Only approved accounts can log in.");
            }

            var token = await _tokenService.CreateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Roles = roles,
                AccountStatus = user.AccountStatus.ToString()
            };
        }
    }
}