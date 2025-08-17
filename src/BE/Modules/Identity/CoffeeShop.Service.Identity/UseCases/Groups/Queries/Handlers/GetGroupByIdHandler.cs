using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Queries.Handlers
{
    public sealed class GetGroupByIdHandler : IRequestHandler<GetGroupByIdQuery, Response<RolesResponse>>
    {
        private readonly IRoleService _roleService;

        public GetGroupByIdHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<Response<RolesResponse>> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetRoleWithPermissionsByIdAsync(request.Id, cancellationToken);
        }
    }
}