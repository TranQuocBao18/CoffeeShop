using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Commands
{
    public partial class UpsertGroupCommand : IRequest<Response<Guid>>
    {
        public GroupRequest? Payload { get; set; }
    }
}