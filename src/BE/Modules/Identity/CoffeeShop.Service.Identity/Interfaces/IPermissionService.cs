using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Model.Dto.Identity.Requests;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.Interfaces
{
    public interface IPermissionService
    {
       Task<Response<IReadOnlyList<PermissionsResponse>>> GetPermissionsAsync(CancellationToken cancellationToken);
        Task<PagedResponse<IList<PermissionsResponse>>> GetPermissionsByRolesAsync(IList<string> roles, CancellationToken cancellationToken = default);
        Task<PagedResponse<IList<PermissionsResponse>>> GetFormattedPermissionsByRolesAsync(IList<string> roles, CancellationToken cancellationToken = default);
        Task<Response<IList<PermissionsResponse>>> GetPermissionsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task RemovePermissionsFromGroup(SystemRole groupFromDb);
        Task UpdatePermissionsFromGroup(IList<PermissionsRequest> permissionsToUpdate, SystemRole groupFromDb);
    }
}