using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands.Handlers
{
    public class DeleteUserByIdHandler : IRequestHandler<DeleteUserByIdCommand, Response<bool>>
    {
        private readonly IUserService _service;

        public DeleteUserByIdHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<Response<bool>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            var userId = request.Id;
            return await _service.DeleteUserAsync(userId, cancellationToken);
        }
    }
}