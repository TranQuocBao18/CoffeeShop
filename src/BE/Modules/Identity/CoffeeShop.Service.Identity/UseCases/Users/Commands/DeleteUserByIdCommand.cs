using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands
{
    public partial class DeleteUserByIdCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }
}