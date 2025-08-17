using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Identity.Queries.Handlers
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, Response<ProfileResponse>>
    {
        private readonly IAccountService _accountService;

        public GetProfileHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Response<ProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = request.Id;
            return await _accountService.GetProfileAsync(userId);
        }
    }
}