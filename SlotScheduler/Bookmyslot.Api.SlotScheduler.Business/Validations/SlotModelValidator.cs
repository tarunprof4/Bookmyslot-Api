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
            Instant now = SystemClock.Instance.GetCurrentInstant();
            var utcZoneTime = now.InUtc();
            Duration timeDifference = utcZoneTime - slotZonedDate;
            if (timeDifference.TotalMinutes > 0)
            {
                return true;
            }
            return false;
        }
     
    }
}
