using System.Linq.Expressions;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Infrastructure.Shared.ErrorCodes;
using CoffeeShop.Infrastructure.Shared.Interfaces;

namespace CoffeeShop.Infrastructure.Identity.Interfaces;

public interface IUserRepository : IGenericRepository<User, Guid>
{
    Task<IReadOnlyList<User>> SearchAsync(Expression<Func<ApplicationUser, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<ErrorCodeEnum> ValidateExistingAsync(string username, string email, Guid? userId = null, bool checkLock = true);
    Task<bool> AllAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken);
    Task<int> CountAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> ResetPasswordAsync(Guid userId, string password);
    Task<bool> CheckPasswordAsync(Guid userId, string oldPassword);
    Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
}
