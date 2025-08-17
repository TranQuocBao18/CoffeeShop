using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.UseCases.Permissions.Queries
{
    public class GetPermissionsQuery : IRequest<Response<IReadOnlyList<PermissionsResponse>>>
    {
    }
}