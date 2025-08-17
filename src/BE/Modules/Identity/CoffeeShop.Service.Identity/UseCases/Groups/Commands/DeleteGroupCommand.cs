using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Commands
{
    public partial class DeleteGroupCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
    }
}