using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Permissions.Queries.Handlers
{
    public class GetPermissionsByUserIdHandler : IRequestHandler<GetPermissionsByUserIdQuery, Response<IList<PermissionsResponse>>>
    {
        private readonly IPermissionService _service;

        public GetPermissionsByUserIdHandler(IPermissionService service)
        {
            _service = service;
        }

        public async Task<Response<IList<PermissionsResponse>>> Handle(GetPermissionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetPermissionsByUserIdAsync(request.UserId, cancellationToken);
        }
    }
}