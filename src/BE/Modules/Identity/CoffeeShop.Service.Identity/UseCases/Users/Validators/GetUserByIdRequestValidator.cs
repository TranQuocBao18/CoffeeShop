using FluentValidation;
using CoffeeShop.Presentation.Shared.ErrorCodes;
using CoffeeShop.Presentation.Shared.Extensions;
using CoffeeShop.Service.Identity.UseCases.Users.Queries;

namespace CoffeeShop.Service.Identity.UseCases.Users.Validators
{
    public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                   .NotNull()
                   .NotEmpty()
                   .WithErrorEnum(CommonValidationCode.MSG_011, "Id");
        }
    }
}