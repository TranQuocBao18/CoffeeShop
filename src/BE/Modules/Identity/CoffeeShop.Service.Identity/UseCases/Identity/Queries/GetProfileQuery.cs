using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.UseCases.Identity.Queries
{
    public class GetProfileQuery : IRequest<Response<ProfileResponse>>
    {
        public Guid Id { get; set; }
    }
}