using CoffeeShop.Model.Dto.Identity.Dtos;

namespace CoffeeShop.Model.Dto.Identity.Responses
{
    public class RolesResponse : RoleDto
    {
        public IList<PermissionsResponse> permissions { get; set; } = [];
    }
}