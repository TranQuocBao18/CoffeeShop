using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.Interfaces
{
    public interface IRoleService
    {
       Task<PagedResponse<IReadOnlyList<RolesResponse>>> GetRolesAsync(CancellationToken cancellationToken);
        Task<PagedResponse<IReadOnlyList<RolesResponse>>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> AssignRolesByUserIdAsync(Guid userId, bool hasAdmin, CancellationToken cancellationToken);
        Task<Response<Guid>> CreateGroup(GroupRequest request, CancellationToken cancellationToken);
        Task<Response<Guid>> UpdateGroup(GroupRequest request, CancellationToken cancellationToken);
        Task<Response<RolesResponse>> GetRoleWithPermissionsByIdAsync(Guid roleId, CancellationToken cancellationToken);
        Task<Response<bool>> GrantPermissions(GrantPermissionRequest request, CancellationToken cancellationToken);
        Task<bool> AssignUserToGroupByUserIdAsync(Guid userId, Guid groupId, CancellationToken cancellationToken);
        Task<Response<Guid>> CloneGroup(CloneGroupRequest request, CancellationToken cancellationToken);
        Task<Response<Guid>> DeleteGroup(Guid groupId, CancellationToken cancellationToken);
    }
}