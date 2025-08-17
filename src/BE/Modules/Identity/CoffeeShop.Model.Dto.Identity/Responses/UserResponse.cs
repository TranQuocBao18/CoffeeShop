using CoffeeShop.Model.Dto.Identity.Dtos;

namespace CoffeeShop.Model.Dto.Identity.Responses
{
    public class UserResponse
    {
        public UserDto? UserDto { get; set; }
        public UserDetailDto? UserDetailDto { get; set; }
    }
}