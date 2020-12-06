using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Bookmyslot.Api.Customers.Business.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.GenderPrefix).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(Constants.GenderPrefixInValid);
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().Must(isNameValid).WithMessage(Constants.FistNameInValid);
            RuleFor(x => x.MiddleName).Cascade(CascadeMode.Stop).NotEmpty().Must(isNameValid).WithMessage(Constants.MiddleNameInValid);
            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().Must(isNameValid).WithMessage(Constants.LastNameInValid);
            RuleFor(x => x.Gender).Cascade(CascadeMode.Stop).NotEmpty().Must(isNameValid).WithMessage(Constants.GenderNotValid);
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().Must(isEmailValid).WithMessage(Constants.EmailIdNotValid);
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
