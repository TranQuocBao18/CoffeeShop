using CoffeeShop.Domain.Identity.Entities;

namespace CoffeeShop.Service.Identity.Interfaces
{
    public interface ITokenService
    {
        RefreshToken GenerateRefreshToken(string ipAddress);

        Task<string> GenerateJWToken(ApplicationUser user);
    }
}