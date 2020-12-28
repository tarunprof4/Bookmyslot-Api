using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace Bookmyslot.Api.Customers.Business.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.FirstNameInValid).Must(isNameValid).WithMessage(AppBusinessMessages.FirstNameInValid);
            RuleFor(x => x.MiddleName).Cascade(CascadeMode.Stop).Must(isNameValid).When(x=> !string.IsNullOrWhiteSpace(x.MiddleName)).WithMessage(AppBusinessMessages.MiddleNameInValid);
            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.LastNameInValid).Must(isNameValid).WithMessage(AppBusinessMessages.LastNameInValid);
            RuleFor(x => x.Gender).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.GenderNotValid).Must(isNameValid).WithMessage(AppBusinessMessages.GenderNotValid);
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.EmailIdNotValid).Must(isEmailValid).WithMessage(AppBusinessMessages.EmailIdNotValid);
        }

        protected override bool PreValidate(ValidationContext<CustomerModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessages.CustomerDetailsMissing));
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
