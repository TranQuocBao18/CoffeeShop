using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Infrastructure.Identity.Contexts;
using CoffeeShop.Infrastructure.Shared.Interfaces.Repositories;

namespace CoffeeShop.Infrastructure.Identity.Services
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DbSet<IdentityRoleClaim<Guid>> _identityRoleClaim;

        public PermissionRepository(IdentityContext dbContext)
        {
            _identityRoleClaim = dbContext.Set<IdentityRoleClaim<Guid>>();
        }

        public async Task<List<Permission>> GetPermissionByRoleIdAsync(Guid roleId)
        {
            // Transfer data of RoleClaim to Permission
            var identityRoleClaims = await _identityRoleClaim.Where(x => x.ClaimType.Equals("permission") && x.RoleId == roleId)
                                                             .ToListAsync();
            if (identityRoleClaims == null || identityRoleClaims.Any())
            {
                return null;
            }

            var permissions = new List<Permission>();
            //var rolePermissionsEnums = await this.GetPermissionsAsync();

            //foreach (var identityRoleClaim in identityRoleClaims)
            //{
            //    var permissionName = rolePermissionsEnums.Where(x => x.Code == identityRoleClaim.ClaimValue)
            //                                             .Select(x => x.Name)
            //                                             .FirstOrDefault();
            //    permissions.Add(new Permission
            //    {
            //        Code = identityRoleClaim.ClaimValue,
            //        Name = permissionName
            //    });
            //}
            return permissions;
        }
    }
}
