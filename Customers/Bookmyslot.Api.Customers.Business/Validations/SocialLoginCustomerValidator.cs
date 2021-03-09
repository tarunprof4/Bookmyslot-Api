using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Constants;
using Bookmyslot.Api.Common.Contracts.Constants;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.Api.Customers.Business.Validations
{
    public class SocialLoginCustomerValidator :  AbstractValidator<SocialCustomerLoginModel>
    {
        public SocialLoginCustomerValidator()
        {
            RuleFor(x => x.AuthToken).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.AuthTokenRequired);
            RuleFor(x => x.Provider).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.TokenProviderRequired);
            
            RuleFor(x => x).Cascade(CascadeMode.Stop).Must(googleValidateIdToken).WithMessage(AppBusinessMessagesConstants.IdTokenRequired);
        }
        protected override bool PreValidate(ValidationContext<SocialCustomerLoginModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.SocialLoginTokenDetailsMissing));
                return false;
            }
            return true;
        }

        private bool googleValidateIdToken(SocialCustomerLoginModel socialCustomerLoginModel)
        {
            if(socialCustomerLoginModel.Provider == LoginConstants.ProviderGoogle)
            {
                var isIdTokenValid = string.IsNullOrWhiteSpace(socialCustomerLoginModel.IdToken);
                return !isIdTokenValid;
            }

            return true;
        }
    }
}
