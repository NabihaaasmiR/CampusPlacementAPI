using CampusPlacementAPI.Authorization.Requirements;
using CampusPlacementAPI.Data;
using CampusPlacementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CampusPlacementAPI.Authorization.Policies
{
    public class SameCompanyAuthorizationHandler : AuthorizationHandler<SameCompanyRequirement, int>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SameCompanyAuthorizationHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SameCompanyRequirement requirement,
            int companyId)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return;

            // SuperAdmin bypasses resource checks
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return;
            }

            if (context.User.IsInRole("CompanyHR"))
            {
                var hrProfile = await _context.CompanyHRs
                    .FirstOrDefaultAsync(hr => hr.UserId == userId);

                if (hrProfile != null && hrProfile.CompanyId == companyId)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}