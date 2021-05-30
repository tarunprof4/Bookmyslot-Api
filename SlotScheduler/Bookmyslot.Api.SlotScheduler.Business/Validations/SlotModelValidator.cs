using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Domain;
using FluentValidation;

namespace Bookmyslot.Api.SlotScheduler.Business.Validations
{
    public class SlotModelValidator : AbstractValidator<SlotModel>
    {
        public SlotModelValidator()
        {
            RuleFor(x => x).Cascade(CascadeMode.Stop).Must(isSlotDateValid).WithMessage(AppBusinessMessagesConstants.InValidSlotDate).Must(slotNotAllowedOnDayLightSavingDay).WithMessage(AppBusinessMessagesConstants.DayLightSavinngDateNotAllowed);
            RuleFor(x => x).Cascade(CascadeMode.Stop).Must(isSlotEndTimeValid).WithMessage(AppBusinessMessagesConstants.SlotEndTimeInvalid);
            RuleFor(x => x).Cascade(CascadeMode.Stop).Must(isSlotDurationValid).WithMessage(AppBusinessMessagesConstants.SlotDurationInvalid);
        }

        private bool isSlotDateValid(SlotModel slotModel)
        {
            return slotModel.IsSlotDateValid();
        }

        private bool slotNotAllowedOnDayLightSavingDay(SlotModel slotModel)
        {
            return slotModel.SlotNotAllowedOnDayLightSavingDay();
        }

        private bool isSlotEndTimeValid(SlotModel slotModel)
        {
            return slotModel.IsSlotEndTimeValid();
        }

        private bool isSlotDurationValid(SlotModel slotModel)
        {
            return slotModel.IsSlotDurationValid();
        }

    }
}
