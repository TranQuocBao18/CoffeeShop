using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Shared.Dtos;

namespace CoffeeShop.Infrastructure.Shared.Interfaces
{
    public interface INotificationPreparationService
{
    Task<Response<GroupUserDto>> GetUsersByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);
}
}