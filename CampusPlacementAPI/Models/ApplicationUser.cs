using Microsoft.AspNetCore.Identity;

namespace CampusPlacementAPI.Models
{
    public enum AccountStatus
    {
        Pending,
        Approved,
        Rejected,
        Suspended,
        Inactive
    }

    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties for user profiles
        public virtual PlacementOfficer? PlacementOfficerProfile { get; set; }
        public virtual CompanyHR? CompanyHRProfile { get; set; }
        public virtual Student? StudentProfile { get; set; }
    }
}