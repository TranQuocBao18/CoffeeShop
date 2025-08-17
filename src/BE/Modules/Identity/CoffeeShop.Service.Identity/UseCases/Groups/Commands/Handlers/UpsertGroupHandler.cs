using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Service.Identity.Interfaces;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Commands.Handlers
{
    public class UpsertGroupHandler : IRequestHandler<UpsertGroupCommand, Response<Guid>>
    {
        private readonly IRoleService _service;

        public UpsertGroupHandler(IRoleService service)
        {
            _service = service;
        }

        public async Task<Response<Guid>> Handle(UpsertGroupCommand request, CancellationToken cancellationToken)
        {
            var groupRequest = request.Payload;

            if (groupRequest == null || groupRequest.Id == null || groupRequest.Id == Guid.Empty)
            {
                return await _service.CreateGroup(groupRequest, cancellationToken);
            }

            return await _service.UpdateGroup(groupRequest, cancellationToken);
        }
    }
}