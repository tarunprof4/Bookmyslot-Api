using Bookmyslot.Api.Common.Contracts.Constants;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Validations
{

    public class SlotSchedulerViewModelValidator : AbstractValidator<SlotSchedulerViewModel>
    {
        public SlotSchedulerViewModelValidator()
        {
            RuleFor(x => x.SlotModelKey).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.SlotScheduleInfoRequired);
        }

        protected override bool PreValidate(ValidationContext<SlotSchedulerViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.SlotScheduleInfoMissing));
                return false;
            }
            return true;
        }



    }
}
