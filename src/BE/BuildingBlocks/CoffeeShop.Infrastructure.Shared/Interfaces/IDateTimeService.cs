namespace CoffeeShop.Infrastructure.Shared.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}