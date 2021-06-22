using Bookmyslot.Api.Common.Contracts.Constants;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Validations
{
    public class ResendSlotInformationViewModelValidator : AbstractValidator<ResendSlotInformationViewModel>
    {
        public ResendSlotInformationViewModelValidator()
        {
            RuleFor(x => x.ResendSlotModel).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.ResendSlotInfoRequired);
        }

        protected override bool PreValidate(ValidationContext<ResendSlotInformationViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.ResendSlotInfoMissing));
                return false;
            }
            return true;
        }



    }
}
