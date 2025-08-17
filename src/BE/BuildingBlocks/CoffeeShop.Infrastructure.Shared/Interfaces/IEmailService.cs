using CoffeeShop.Model.Dto.Shared.Outbox;

namespace CoffeeShop.Infrastructure.Shared.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}