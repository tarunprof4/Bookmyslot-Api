using Bookmyslot.Api.NodaTime.Interfaces;
using Bookmyslot.SharedKernel.Constants;
using Bookmyslot.SharedKernel.ExtensionMethods;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Validations
{
    public class SlotViewModelValidator : AbstractValidator<SlotViewModel>
    {
        private readonly INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness;

        public SlotViewModelValidator(INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness)
        {
            this.nodaTimeZoneLocationBusiness = nodaTimeZoneLocationBusiness;
            RuleFor(x => x.Title).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotTitleRequired);
            RuleFor(x => x.Country).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.CountryRequired).Must(isCountryValid).WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.InValidCountry);
            RuleFor(x => x.TimeZone).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.TimeZoneRequired).Must(isTimeZoneValid).WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.InValidTimeZone);
            RuleFor(x => x.SlotDate).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.SlotDateRequired).Must(isSlotDateValid).WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.InValidSlotDate);
        }

        protected override bool PreValidate(ValidationContext<SlotViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, Common.Contracts.Constants.AppBusinessMessagesConstants.SlotDetailsMissing));
                return false;
            }
            return true;
        }


        private bool isCountryValid(string country)
        {
            var nodaTimeZoneLocationConfiguration = this.nodaTimeZoneLocationBusiness.GetNodaTimeZoneLocationInformation();
            return nodaTimeZoneLocationConfiguration.IsCountryValid(country);
        }

        private bool isTimeZoneValid(string timeZone)
        {
            var nodaTimeZoneLocationConfiguration = this.nodaTimeZoneLocationBusiness.GetNodaTimeZoneLocationInformation();
            return nodaTimeZoneLocationConfiguration.IsTimeZoneValid(timeZone);
        }


        private bool isSlotDateValid(string slotDate)
        {
            return slotDate.isDateValid(DateTimeConstants.ApplicationDatePattern);
        }

    }
}
