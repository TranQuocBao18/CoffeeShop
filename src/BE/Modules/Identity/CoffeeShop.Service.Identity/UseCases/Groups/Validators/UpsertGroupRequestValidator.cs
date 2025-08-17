using FluentValidation;
using CoffeeShop.Presentation.Shared.ErrorCodes;
using CoffeeShop.Presentation.Shared.Extensions;
using CoffeeShop.Service.Identity.UseCases.Groups.Commands;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Validators
{
    public class UpsertGroupRequestValidator : AbstractValidator<UpsertGroupCommand>
    {
        public UpsertGroupRequestValidator()
        {
            RuleFor(x => x.Payload)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, "Payload");

            RuleFor(x => x.Payload.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, "Name");
        }
    }
}