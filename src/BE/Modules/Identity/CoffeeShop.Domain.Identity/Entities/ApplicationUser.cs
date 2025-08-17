using Microsoft.AspNetCore.Identity;

namespace CoffeeShop.Domain.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Code { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; } = DateTime.Now;
        public virtual List<RefreshToken>? RefreshTokens { get; set; }
        public virtual List<UserSetting>? UserSettings { get; set; }
    }
}
