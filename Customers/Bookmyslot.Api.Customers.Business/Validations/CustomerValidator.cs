using Bookmyslot.Api.Customers.Contracts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Business.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.GenderPrefix).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please specify a first name");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please specify a last name");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Please specify gender");
            RuleFor(x => x.Email).Must(BeAValidEmailId).WithMessage("Please specify a valid email id");
        }

        private bool BeAValidEmailId(string email)
        {
            return true;
        }
    }

}
