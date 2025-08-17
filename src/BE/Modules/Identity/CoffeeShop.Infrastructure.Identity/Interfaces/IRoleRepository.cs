using System.Linq.Expressions;
using CoffeeShop.Domain.Identity.Entities;

namespace CoffeeShop.Infrastructure.Identity.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> GetRolesAsync();
    Task<List<Role>> GetRolesAsync(Expression<Func<SystemRole, bool>> predicate, CancellationToken cancellationToken);
    Task<List<Role>> GetRolesByUserIdAsync(Guid userId);
    Task<bool> GrantRolesByUserIdAsync(Guid userId, List<string> roleNames);
    Task<Guid> CreateRoleWithCodeAsync(SystemRole role, CancellationToken cancellationToken);
    Task<bool> RemovePermissionsById(Guid roleId, IEnumerable<string> permissions, bool hasTransaction = false);
    Task<bool> AddUserToGroupByUserIdAsync(User user, SystemRole group, bool hasTransaction = false);
    Task<bool> RemoveUserFromGroupByUserIdAsync(User user, Role group, bool hasTransaction = false);
    Task<bool> AnyUserInGroupAsync(string groupName);
    Task<bool> DeleteGroup(string groupName, CancellationToken cancellationToken = default!, bool hasTransaction = false);
    Task<IList<string>> GetUserIdsByGroupIdAsync(string groupId, CancellationToken cancellationToken);
}
