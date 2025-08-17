using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands
{
    public partial class ResetPasswordUserCommand : IRequest<Response<string>>
    {
        public Guid UserId { get; set; }
    }
}