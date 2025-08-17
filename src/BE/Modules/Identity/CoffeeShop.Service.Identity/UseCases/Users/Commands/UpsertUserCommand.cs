using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands
{
    public partial class UpsertUserCommand : IRequest<Response<Guid>>
    {
        public UserRequest? Payload { get; set; }
    }
}