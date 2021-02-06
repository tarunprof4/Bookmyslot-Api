using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace Bookmyslot.Api.Customers.Business.Validations
{
    public class ProfileSettingsValidator :  AbstractValidator<ProfileSettingsModel>
    {
        public ProfileSettingsValidator()
        {
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.FirstNameInValid).Must(isNameValid).WithMessage(AppBusinessMessagesConstants.FirstNameInValid);
            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.LastNameInValid).Must(isNameValid).WithMessage(AppBusinessMessagesConstants.LastNameInValid);
            RuleFor(x => x.Gender).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.GenderNotValid).Must(areAlphabets).WithMessage(AppBusinessMessagesConstants.GenderNotValid);
            RuleFor(x => x.BioHeadLine).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.BioHeadLineNotValid);
        }
        protected override bool PreValidate(ValidationContext<ProfileSettingsModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.ProfileSettingDetailsMissing));
                return false;
            }
            return true;
        }

        private bool isNameValid(string name)
        {
            return Regex.IsMatch(name, Regexes.Name);
        }

        private bool areAlphabets(string name)
        {
            return Regex.IsMatch(name, Regexes.Alphabets);
        }

    }
}
