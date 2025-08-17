using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Queries
{
    public class GetGroupByIdQuery : IRequest<Response<RolesResponse>>
    {
        public Guid Id { get; set; }
    }
}