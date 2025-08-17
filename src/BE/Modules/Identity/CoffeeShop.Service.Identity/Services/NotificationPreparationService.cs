using CoffeeShop.Infrastructure.Identity.Interfaces;
using CoffeeShop.Infrastructure.Shared.ErrorCodes;
using CoffeeShop.Infrastructure.Shared.Interfaces;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Shared.Dtos;
using CoffeeShop.Utilities.Extensions;

namespace CoffeeShop.Service.Identity.Services
{
    public class NotificationPreparationService : INotificationPreparationService
{
    private readonly IIdentityUnitOfWork _identityUnitOfWork;

    public NotificationPreparationService(IIdentityUnitOfWork identityUnitOfWork)
    {
        _identityUnitOfWork = identityUnitOfWork;
    }

    public async Task<Response<GroupUserDto>> GetUsersByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        var groupFromDb = await _identityUnitOfWork.RoleRepository.GetRolesAsync(g => g.Id == groupId.ToString(), cancellationToken);
        if (groupFromDb == null)
            return new Response<GroupUserDto>(ErrorCodeEnum.ROG_ERR_001);
        var groupUsers = await _identityUnitOfWork.RoleRepository.GetUserIdsByGroupIdAsync(groupId.ToString(), cancellationToken);
        var response = new GroupUserDto
        {
            GroupId = groupId,
            UserIds = groupUsers.Select(x => x.AsGuid()).ToList()
        };

        return new Response<GroupUserDto>(response);
    }
}
}