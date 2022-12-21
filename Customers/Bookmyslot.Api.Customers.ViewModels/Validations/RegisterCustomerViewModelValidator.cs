using Bookmyslot.SharedKernel.Constants;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace Bookmyslot.Api.Customers.ViewModels.Validations
{

    public class RegisterCustomerViewModelValidator : AbstractValidator<RegisterCustomerViewModel>
    {
        public RegisterCustomerViewModelValidator()
        {
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.FirstNameRequired).Must(isNameValid).WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.FirstNameInValid);
            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.LastNameRequired).Must(isNameValid).WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.LastNameInValid);
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.EmailRequired).Must(isEmailValid).WithMessage(Common.Contracts.Constants.AppBusinessMessagesConstants.EmailIdNotValid);
        }

        protected override bool PreValidate(ValidationContext<RegisterCustomerViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, Common.Contracts.Constants.AppBusinessMessagesConstants.RegisterCustomerDetailsMissing));
                return false;
            }
            return true;
        }

        private bool isNameValid(string name)
        {
            return Regex.IsMatch(name, RegexConstants.Name);
        }

        private bool isEmailValid(string email)
        {
            return Regex.IsMatch(email, RegexConstants.Email);
        }
    }
}
