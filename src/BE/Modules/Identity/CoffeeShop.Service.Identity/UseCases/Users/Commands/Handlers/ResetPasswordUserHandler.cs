using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands.Handlers
{
    public class ResetPasswordUserHandler : IRequestHandler<ResetPasswordUserCommand, Response<string>>
    {
        private readonly IUserService _service;

        public ResetPasswordUserHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<Response<string>> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            return await _service.ResetPasswordUserAsync(request.UserId, cancellationToken);
        }
    }
}