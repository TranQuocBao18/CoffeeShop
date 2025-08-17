using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands.Handlers
{
    public class ChangePasswordUserHandler : IRequestHandler<ChangePasswordUserCommand, Response<bool>>
    {
        private readonly IUserService _service;

        public ChangePasswordUserHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<Response<bool>> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
        {
            return await _service.ChangePasswordUserAsync(request.OldPassword, request.NewPassword, cancellationToken);
        }
    }
}