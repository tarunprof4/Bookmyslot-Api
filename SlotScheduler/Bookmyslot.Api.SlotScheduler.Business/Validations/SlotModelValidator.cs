using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Constants;
using FluentValidation;
using NodaTime;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.Validations
{
    public class SlotModelValidator : AbstractValidator<SlotModel>
    {
        public SlotModelValidator()
        {
            RuleFor(x => x.SlotZonedDate).Cascade(CascadeMode.Stop).Must(isSlotDateValid).WithMessage(AppBusinessMessagesConstants.InValidSlotDate);
            RuleFor(x => x).Cascade(CascadeMode.Stop).Must(isSlotEndTimeValid).WithMessage(AppBusinessMessagesConstants.SlotEndTimeInvalid);
            RuleFor(x => x.SlotDuration).Cascade(CascadeMode.Stop).Must(isSlotDurationValid).WithMessage(AppBusinessMessagesConstants.SlotDurationInvalid);
        }

        private bool isSlotDateValid(ZonedDateTime slotZonedDate)
        {
            var utcZoneTime = SystemClock.Instance.GetCurrentInstant().InUtc();
            Duration timeDifference = slotZonedDate - utcZoneTime;
            if (timeDifference.TotalHours > 0)
            {
                return true;
            }
            return false;
        }

        private bool isSlotEndTimeValid(SlotModel slotModel)
        {
            if (slotModel.SlotEndTime > slotModel.SlotStartTime)
            {
                return true;
            }
            return false;
        }

        private bool isSlotDurationValid(TimeSpan slotDuration)
        {
            if (slotDuration.TotalMinutes >= SlotConstants.MinimumSlotDuration)
            {
                return true;
            }
            return false;
        }

    }
}
