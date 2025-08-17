using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Commands.Handlers
{
    public class GrantPermissionHandler : IRequestHandler<GrantPermissionCommand, Response<bool>>
    {
        private readonly IRoleService _roleService;

        public GrantPermissionHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<Response<bool>> Handle(GrantPermissionCommand request, CancellationToken cancellationToken)
        {
            return await _roleService.GrantPermissions(request.Payload, cancellationToken);
        }
    }
}