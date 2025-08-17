using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.UseCases.Users.Queries
{
    public class GetUserByIdQuery : IRequest<Response<UserResponse>>
    {
        public Guid Id { get; set; }
    }
}