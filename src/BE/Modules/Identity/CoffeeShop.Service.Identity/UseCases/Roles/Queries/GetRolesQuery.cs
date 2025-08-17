using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.UseCases.Roles.Queries
{
    public class GetRolesQuery : IRequest<PagedResponse<IReadOnlyList<RolesResponse>>>
    {
    }
}