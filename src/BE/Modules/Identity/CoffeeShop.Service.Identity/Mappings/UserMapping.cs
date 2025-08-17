using AutoMapper;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Model.Dto.Identity.Dtos;
using CoffeeShop.Model.Dto.Identity.Requests;
using CoffeeShop.Model.Dto.Identity.Responses;

namespace CoffeeShop.Service.Identity.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserDetailDto>().ReverseMap();
            CreateMap<User, UsersResponse>();
            CreateMap<UserRequest, User>();
        }
    }
}