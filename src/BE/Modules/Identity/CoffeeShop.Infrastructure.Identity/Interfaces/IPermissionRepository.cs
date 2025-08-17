using CoffeeShop.Domain.Identity.Entities;

namespace CoffeeShop.Infrastructure.Shared.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetPermissionByRoleIdAsync(Guid roleId);
    }
}
