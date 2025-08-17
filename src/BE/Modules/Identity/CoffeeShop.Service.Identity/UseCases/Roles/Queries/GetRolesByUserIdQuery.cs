using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.UseCases.Roles.Queries
{
    public class GetRolesByUserIdQuery : IRequest<PagedResponse<IReadOnlyList<RolesResponse>>>
    {
        public Guid UserId { get; set; }
    }
}