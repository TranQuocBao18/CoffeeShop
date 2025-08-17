using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.UseCases.Permissions.Queries
{
    public class GetPermissionsByUserIdQuery : IRequest<Response<IList<PermissionsResponse>>>
    {
        public Guid UserId { get; set; }
    }
}