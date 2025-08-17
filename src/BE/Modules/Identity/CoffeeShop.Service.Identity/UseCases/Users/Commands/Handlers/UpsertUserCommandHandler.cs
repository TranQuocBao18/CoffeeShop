using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands.Handlers
{
    public class UpsertUserCommandHandler : IRequestHandler<UpsertUserCommand, Response<Guid>>
    {
        private readonly IUserService _service;

        public UpsertUserCommandHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<Response<Guid>> Handle(UpsertUserCommand request, CancellationToken cancellationToken)
        {
            var userRequest = request.Payload;
            if (userRequest == null || userRequest.Id == null || userRequest.Id == Guid.Empty)
            {
                return await _service.CreateUserAsync(userRequest, cancellationToken);
            }
            else
            {
                return await _service.UpdateUserAsync(userRequest, cancellationToken);
            }
        }
    }
}