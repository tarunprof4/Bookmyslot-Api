using Bookmyslot.Api.Common.Contracts.Constants;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace Bookmyslot.Api.Customers.ViewModels.Validations
{
    public class ProfileSettingsViewModelValidator : AbstractValidator<ProfileSettingsViewModel>
    {
        public ProfileSettingsViewModelValidator()
        {
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.FirstNameRequired).Must(isNameValid).WithMessage(AppBusinessMessagesConstants.FirstNameInValid);
            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.LastNameRequired).Must(isNameValid).WithMessage(AppBusinessMessagesConstants.LastNameInValid);
            RuleFor(x => x.Gender).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.GenderRequired).Must(areAlphabets).WithMessage(AppBusinessMessagesConstants.GenderNotValid);
        }

        protected override bool PreValidate(ValidationContext<ProfileSettingsViewModel> context, ValidationResult result)
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
            return Regex.IsMatch(name, RegexConstants.Name);
        }

        private bool areAlphabets(string name)
        {
            return Regex.IsMatch(name, RegexConstants.Alphabets);
        }
    }
}
