

using Bookmyslot.Api.Common.Contracts.Constants;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.Api.Customers.ViewModels.Validations
{

    public class AdditionalProfileSettingsViewModelValidator : AbstractValidator<AdditionalProfileSettingsViewModel>
    {
        public AdditionalProfileSettingsViewModelValidator()
        {
            RuleFor(x => x.BioHeadLine).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.BioHeadLineRequired);
        }

        protected override bool PreValidate(ValidationContext<AdditionalProfileSettingsViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.AdditionalProfileSettingDetailsMissing));
                return false;
            }
            return true;
        }
    }
}
