using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Roles.Queries.Handlers
{
    public class GetRolesHandler : IRequestHandler<GetRolesQuery, PagedResponse<IReadOnlyList<RolesResponse>>>
    {
        private readonly IRoleService _service;

        public GetRolesHandler(IRoleService service)
        {
            _service = service;
        }

        public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetRolesAsync(cancellationToken);
        }
    }
}