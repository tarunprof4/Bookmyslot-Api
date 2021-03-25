using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.NodaTime.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.Api.Customers.ViewModels.Validations
{
    public class CustomerSettingsViewModelValidator : AbstractValidator<CustomerSettingsViewModel>
    {
        private readonly INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness;

        public CustomerSettingsViewModelValidator(INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness)
        {
            this.nodaTimeZoneLocationBusiness = nodaTimeZoneLocationBusiness;
            RuleFor(x => x.Country).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.CountryRequired).Must(isCountryValid).WithMessage(AppBusinessMessagesConstants.InValidCountry);
            RuleFor(x => x.TimeZone).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.TimeZoneRequired).Must(isTimeZoneValid).WithMessage(AppBusinessMessagesConstants.InValidTimeZone);
        }

        protected override bool PreValidate(ValidationContext<CustomerSettingsViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.CustomerSettingsMissing));
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
    }
}
