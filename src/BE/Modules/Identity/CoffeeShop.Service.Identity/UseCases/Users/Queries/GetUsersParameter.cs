using CoffeeShop.Model.Dto.Shared.Filters;

namespace CoffeeShop.Service.Identity.UseCases.Users.Queries
{
    public class GetUsersParameter : RequestParameter
    {
    }
    public class SearchUsersParameter : RequestParameter
    {
        public string? Search { get; set; }
    }
}