using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Queries
{
    public class GetGroupsQuery : IRequest<PagedResponse<IReadOnlyList<RolesResponse>>>
    {
    }
}