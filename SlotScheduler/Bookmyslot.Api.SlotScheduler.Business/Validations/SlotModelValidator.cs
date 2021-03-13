using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Constants;
using FluentValidation;
using NodaTime;

namespace Bookmyslot.Api.SlotScheduler.Business.Validations
{
    public class SlotModelValidator : AbstractValidator<SlotModel>
    {
        public SlotModelValidator()
        {
            RuleFor(x => x.SlotZonedDate).Cascade(CascadeMode.Stop).Must(isSlotDateValid).WithMessage(AppBusinessMessagesConstants.InValidSlotDate);
            RuleFor(x => x.SlotEndTime.TotalMinutes).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(x => x.SlotStartTime.TotalMinutes + SlotConstants.MinimumSlotDuration).WithMessage(AppBusinessMessagesConstants.SlotEndTimeInvalid);
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
     
    }
}
