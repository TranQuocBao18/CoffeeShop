using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Commands
{
    public class CloneGroupCommand : IRequest<Response<Guid>>
    {
        public CloneGroupRequest Payload { get; set; } = default!;
    }
}