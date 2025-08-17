using FluentValidation;
using CoffeeShop.Service.Identity.UseCases.Permissions.Queries;

namespace CoffeeShop.Service.Identity.UseCases.Permissions.Validators
{
    public class GetPermissionsByUserIdValidator : AbstractValidator<GetPermissionsByUserIdQuery>
    {
        public GetPermissionsByUserIdValidator()
        {
            RuleFor(p => p.UserId)
              .NotEmpty()
              .NotNull();
        }
    }
}