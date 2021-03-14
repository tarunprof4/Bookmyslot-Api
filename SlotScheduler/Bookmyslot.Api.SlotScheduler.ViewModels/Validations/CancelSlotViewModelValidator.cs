using Bookmyslot.Api.Common.Contracts.Constants;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Validations
{
    public class CancelSlotViewModelValidator : AbstractValidator<CancelSlotViewModel>
    {
        public CancelSlotViewModelValidator()
        {
            RuleFor(x => x.SlotKey).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessagesConstants.CancelSlotRequired);
        }

        protected override bool PreValidate(ValidationContext<CancelSlotViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.CancelSlotInfoMissing));
                return false;
            }
            return true;
        }



    }
}
