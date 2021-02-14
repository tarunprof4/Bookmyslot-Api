using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace Bookmyslot.Api.Customers.Business.Validations
{
    public class RegisterCustomerValidator :  AbstractValidator<RegisterCustomerModel>
    {
        public RegisterCustomerValidator()
        {
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.FirstNameInValid).Must(isNameValid).WithMessage(AppBusinessMessagesConstants.FirstNameInValid);
            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.LastNameInValid).Must(isNameValid).WithMessage(AppBusinessMessagesConstants.LastNameInValid);
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.EmailIdNotValid).Must(isEmailValid).WithMessage(AppBusinessMessagesConstants.EmailIdNotValid);
        }
        protected override bool PreValidate(ValidationContext<RegisterCustomerModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.RegisterCustomerDetailsMissing));
                return false;
            }
            return true;
        }

        private bool isNameValid(string name)
        {
            return Regex.IsMatch(name, Regexes.Name);
        }

        private bool isEmailValid(string email)
        {
            return Regex.IsMatch(email, Regexes.Email);
        }
    }
}
