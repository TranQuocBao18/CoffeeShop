using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Commands
{
    public class GrantPermissionCommand : IRequest<Response<bool>>
    {
        public GrantPermissionRequest Payload { get; set; } = default!;
    }
}