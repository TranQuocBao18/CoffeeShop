using FluentValidation;
using CoffeeShop.Service.Identity.UseCases.Roles.Queries;

namespace CoffeeShop.Service.Identity.UseCases.Roles.Validators
{
    public class GetRolesByUserIdValidator : AbstractValidator<GetRolesByUserIdQuery>
    {
        public GetRolesByUserIdValidator()
        {
            RuleFor(p => p.UserId)
              .NotEmpty()
              .NotNull();
        }
    }
}