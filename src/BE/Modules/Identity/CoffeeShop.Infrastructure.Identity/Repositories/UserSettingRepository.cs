using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Infrastructure.Identity.Contexts;
using CoffeeShop.Infrastructure.Identity.Interfaces;
using CoffeeShop.Infrastructure.Identity.Repositories;
using CoffeeShop.Infrastructure.Shared.Interfaces.Repositories;

namespace CoffeeShop.Infrastructure.Identity.Services
{
    public class UserSettingRepository : IUserSettingRepository
    {
        private readonly DbSet<UserSetting> _userSettings;

        public UserSettingRepository(IdentityContext dbContext)
        {
            _userSettings = dbContext.Set<UserSetting>();
        }

        public async Task<UserSetting> GetByKeyAsync(string key, CancellationToken cancellationToken)
        {
            return await _userSettings.FindAsync(key, cancellationToken);
        }
    }
}
