using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Roles.Queries.Handlers
{
    public class GetRolesByUserIdHandler : IRequestHandler<GetRolesByUserIdQuery, PagedResponse<IReadOnlyList<RolesResponse>>>
    {
        private readonly IRoleService _service;

        public GetRolesByUserIdHandler(IRoleService service)
        {
            _service = service;
        }

        public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetRolesAsync(cancellationToken);
        }
    }
}