using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Users.Queries.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, PagedResponse<IReadOnlyList<UsersResponse>>>
    {
        private readonly IUserService _service;

        public GetUsersHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<PagedResponse<IReadOnlyList<UsersResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetUsersAsync(request.PageNumber, request.PageSize, request.Search, cancellationToken);
        }
    }
}