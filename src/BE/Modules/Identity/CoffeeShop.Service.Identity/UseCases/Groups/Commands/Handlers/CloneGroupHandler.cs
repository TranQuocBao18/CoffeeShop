using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Commands.Handlers
{
    public class CloneGroupHandler : IRequestHandler<CloneGroupCommand, Response<Guid>>
    {
        private readonly IRoleService _roleService;

        public CloneGroupHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<Response<Guid>> Handle(CloneGroupCommand request, CancellationToken cancellationToken)
        {
            return await _roleService.CloneGroup(request.Payload, cancellationToken);
        }
    }
}