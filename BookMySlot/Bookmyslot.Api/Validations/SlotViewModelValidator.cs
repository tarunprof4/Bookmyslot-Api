using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.ExtensionMethods;
using Bookmyslot.Api.Location.Interfaces;
using Bookmyslot.Api.ViewModels;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.Api.Validations
{
    public class SlotViewModelValidator : AbstractValidator<SlotViewModel>
    {
        private readonly INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness;

        public SlotViewModelValidator(INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness)
        {
            this.nodaTimeZoneLocationBusiness = nodaTimeZoneLocationBusiness;
            RuleFor(x => x.Title).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.SlotTitleRequired);
            RuleFor(x => x.TimeZone).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.TimeZoneRequired).Must(isTimeZoneValid).WithMessage(AppBusinessMessagesConstants.InValidTimeZone);
            RuleFor(x => x.SlotDate).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.SlotDateRequired).Must(isSlotDateValid).WithMessage(AppBusinessMessagesConstants.InValidSlotDate);
        }

        protected override bool PreValidate(ValidationContext<SlotViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.SlotDetailsMissing));
                return false;
            }
            return true;
        }


        private bool isTimeZoneValid(string timeZone)
        {
            var nodaTimeZoneLocationConfiguration = this.nodaTimeZoneLocationBusiness.GetNodaTimeZoneLocationInformation();
            if (nodaTimeZoneLocationConfiguration.ZoneWithCountryId.ContainsKey(timeZone))
            {
                return true;
            }

            return false;
        }


        private bool isSlotDateValid(string slotDate)
        {
            return slotDate.isDateValid();
        }

    }
}
