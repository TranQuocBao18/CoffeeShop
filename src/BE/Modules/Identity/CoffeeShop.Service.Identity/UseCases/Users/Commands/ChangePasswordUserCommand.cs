using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands
{
    public partial class ChangePasswordUserCommand : IRequest<Response<bool>>
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}