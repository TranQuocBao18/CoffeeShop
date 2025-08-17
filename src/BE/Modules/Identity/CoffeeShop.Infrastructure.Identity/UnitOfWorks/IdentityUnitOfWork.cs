using CoffeeShop.Infrastructure.Identity.Contexts;
using CoffeeShop.Infrastructure.Identity.Interfaces;
using CoffeeShop.UnitOfWork;

namespace CoffeeShop.Infrastructure.Identity.UnitOfWorks;

public class IdentityUnitOfWork : BaseUnitOfWork, IIdentityUnitOfWork
{
    public IUserRepository UserRepository { get; private set; }
    public IRoleRepository RoleRepository { get; private set; }

    public IdentityUnitOfWork(
        IdentityContext context, 
        IUserRepository userRepository, 
        IRoleRepository roleRepository) : base(context)
    {
        UserRepository = userRepository;
        RoleRepository = roleRepository;
    }
}
