using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Users.Queries.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Response<UserResponse>>
    {
        private readonly IUserService _service;

        public GetUserByIdHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<Response<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = request.Id;
            return await _service.GetUserByIdAsync(userId, cancellationToken);
        }
    }
}