using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Constants;
using FluentValidation;
using FluentValidation.Results;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.Validations
{
    public class SlotValidator : AbstractValidator<SlotModel>
    {
        public SlotValidator(DateTime currentDate)
        {
            RuleFor(x => x.Title).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.SlotTitleMissing);
            RuleFor(x => x.CreatedBy).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.UserIdMissing);
            RuleFor(x => x.TimeZone).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.TimeZoneMissing);
            RuleFor(x => x.SlotDateUtc).Cascade(CascadeMode.Stop).GreaterThan(x => currentDate).WithMessage(AppBusinessMessagesConstants.SlotStartDateInvalid);
            RuleFor(x => x.SlotEndTime.TotalMinutes).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(x => x.SlotStartTime.TotalMinutes + SlotConstants.MinimumSlotDuration).WithMessage(AppBusinessMessagesConstants.SlotEndTimeInvalid);
        }

        protected override bool PreValidate(ValidationContext<SlotModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.SlotDetailsMissing));
                return false;
            }
            return true;
        }

    }
}
