﻿using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace Bookmyslot.Api.Customers.Business.Validations
{
    public class SocialLoginCustomerValidator :  AbstractValidator<SocialCustomerModel>
    {
        public SocialLoginCustomerValidator()
        {
            RuleFor(x => x.Token).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.TokenRequired);
            RuleFor(x => x.Provider).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.TokenProviderRequired);
        }
        protected override bool PreValidate(ValidationContext<SocialCustomerModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.SocialLoginTokenDetailsMissing));
                return false;
            }
            return true;
        }

      
    }
}
