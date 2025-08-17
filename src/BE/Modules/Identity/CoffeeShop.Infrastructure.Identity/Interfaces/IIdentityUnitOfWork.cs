using CoffeeShop.Infrastructure.Identity.Interfaces;
using CoffeeShop.UnitOfWork;

namespace CoffeeShop.Infrastructure.Identity.Interfaces;

public interface IIdentityUnitOfWork : IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
}
