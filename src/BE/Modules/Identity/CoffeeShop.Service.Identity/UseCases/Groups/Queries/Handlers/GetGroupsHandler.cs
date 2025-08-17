using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Queries.Handlers
{
    public sealed class GetGroupsHandler : IRequestHandler<GetGroupsQuery, PagedResponse<IReadOnlyList<RolesResponse>>>
    {
        private readonly IRoleService _roleService;

        public GetGroupsHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetRolesAsync(cancellationToken);
        }
    }
}