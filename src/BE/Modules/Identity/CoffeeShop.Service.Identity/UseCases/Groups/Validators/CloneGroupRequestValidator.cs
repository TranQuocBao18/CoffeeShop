using FluentValidation;
using CoffeeShop.Presentation.Shared.ErrorCodes;
using CoffeeShop.Presentation.Shared.Extensions;
using CoffeeShop.Service.Identity.UseCases.Groups.Commands;

namespace CoffeeShop.Service.Identity.UseCases.Groups.Validators
{
    public class CloneGroupRequestValidator : AbstractValidator<CloneGroupCommand>
    {
        public CloneGroupRequestValidator()
        {
            RuleFor(x => x.Payload)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, nameof(CloneGroupCommand.Payload));

            RuleFor(x => x.Payload.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithErrorEnum(CommonValidationCode.MSG_011, nameof(CloneGroupCommand.Payload.Id));
        }
    }
}