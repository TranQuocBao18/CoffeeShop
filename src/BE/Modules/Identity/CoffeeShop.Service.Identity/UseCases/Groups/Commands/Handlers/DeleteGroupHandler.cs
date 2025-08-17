using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Commands.Handlers
{
    public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, Response<Guid>>
    {
        private readonly IRoleService _roleService;

        public DeleteGroupHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<Response<Guid>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            return await _roleService.DeleteGroup(request.Id, cancellationToken);
        }
    }
}