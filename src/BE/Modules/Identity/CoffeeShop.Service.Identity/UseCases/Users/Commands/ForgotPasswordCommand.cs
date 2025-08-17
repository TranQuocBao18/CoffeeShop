using MediatR;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;

namespace CoffeeShop.Service.Identity.UseCases.Users.Commands
{
    public partial class ForgotPasswordCommand : IRequest<Response<bool>>
    {
        public ForgotPasswordRequest Payload { get; set; }
        public string IPAddress { get; set; }
    }
}