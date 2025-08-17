using FluentValidation;
using CoffeeShop.Model.Dto.Identity.Requests;
using CoffeeShop.Presentation.Shared.ErrorCodes;
using CoffeeShop.Presentation.Shared.Extensions;
using CoffeeShop.Service.Identity.UseCases.Users.Commands;

namespace CoffeeShop.Service.Identity.UseCases.Users.Validators
{
    public class UpsertUserRequestValidator : AbstractValidator<UpsertUserCommand>
    {
        public UpsertUserRequestValidator()
        {
            RuleFor(x => x.Payload)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_019, "Payload");

            RuleFor(x => x.Payload.Username)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_019, nameof(UserRequest.Username));

            RuleFor(x => x.Payload.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_019, nameof(UserRequest.Email));
        }
    }
}