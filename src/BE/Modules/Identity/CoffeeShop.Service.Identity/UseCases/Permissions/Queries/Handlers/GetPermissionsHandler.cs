using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Permissions.Queries.Handlers
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, Response<IReadOnlyList<PermissionsResponse>>>
    {
        private readonly IPermissionService _service;

        public GetPermissionsHandler(IPermissionService service)
        {
            _service = service;
        }

        public async Task<Response<IReadOnlyList<PermissionsResponse>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetPermissionsAsync(cancellationToken);
        }
    }
}