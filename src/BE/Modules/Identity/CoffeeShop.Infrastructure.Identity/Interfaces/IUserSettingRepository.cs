using CoffeeShop.Domain.Identity.Entities;

namespace CoffeeShop.Infrastructure.Identity.Interfaces
{
    public interface IUserSettingRepository
    {
        Task<UserSetting> GetByKeyAsync(string key, CancellationToken cancellationToken);
    }
}
