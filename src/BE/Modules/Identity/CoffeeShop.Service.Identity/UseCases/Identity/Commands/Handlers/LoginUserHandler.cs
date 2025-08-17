using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Identity.Commands.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Response<AuthenticationResponse>>
    {
        private readonly IAccountService _accountService;

        public LoginUserHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Response<AuthenticationResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var authenticationRequest = new AuthenticationRequest()
            {
                Email = request.Email,
                Password = request.Password,
                RememberMe = request.RememberMe,
            };
            return await _accountService.AuthenticateAsync(authenticationRequest, request.IPAddress);
        }
    }
}