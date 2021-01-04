using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using FluentValidation;
using FluentValidation.Results;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.Validations
{
    public class SlotValidator : AbstractValidator<SlotModel>
    {
        public SlotValidator(DateTime currentDate)
        {
            RuleFor(x => x.Title).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.SlotTitleMissing);
            RuleFor(x => x.CreatedBy).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.UserIdMissing);
            RuleFor(x => x.TimeZone).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.TimeZoneMissing);
            RuleFor(x => x.SlotDate.Date).Cascade(CascadeMode.Stop).GreaterThan(x => currentDate.Date).WithMessage(AppBusinessMessages.SlotStartDateInvalid);
            RuleFor(x => x.SlotEndTime.TotalMinutes).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(x => x.SlotStartTime.TotalMinutes + SlotConstants.MinimumSlotDuration).WithMessage(AppBusinessMessages.SlotEndTimeInvalid);
        }

        protected override bool PreValidate(ValidationContext<SlotModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessages.SlotDetailsMissing));
                return false;
            }
            return true;
        }

    }
}
