using CoffeeShop.Infrastructure.Shared.Interfaces;

namespace CoffeeShop.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}