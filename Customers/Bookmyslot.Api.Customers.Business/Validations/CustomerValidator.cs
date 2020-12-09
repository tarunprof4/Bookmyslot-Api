using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Bookmyslot.Api.Customers.Business.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.GenderPrefix).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.GenderPrefixInValid);
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.FirstNameInValid).Must(isNameValid).WithMessage(AppBusinessMessages.FirstNameInValid);
            RuleFor(x => x.MiddleName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.MiddleNameInValid).Must(isNameValid).WithMessage(AppBusinessMessages.MiddleNameInValid);
            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.LastNameInValid).Must(isNameValid).WithMessage(AppBusinessMessages.LastNameInValid);
            RuleFor(x => x.Gender).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.GenderNotValid).Must(isNameValid).WithMessage(AppBusinessMessages.GenderNotValid);
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.EmailIdNotValid).Must(isEmailValid).WithMessage(AppBusinessMessages.EmailIdNotValid);
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
