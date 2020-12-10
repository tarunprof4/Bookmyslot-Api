using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using FluentValidation;
using FluentValidation.Results;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.Validations
{
    public class SlotValidator : AbstractValidator<SlotModel>
    {
        public SlotValidator()
        {
            RuleFor(x => x.CreatedBy).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(AppBusinessMessages.UserIdMissing);
            RuleFor(x => x.StartTime).Cascade(CascadeMode.Stop).GreaterThan(x => DateTime.UtcNow).WithMessage(AppBusinessMessages.SlotStartDateInvalid);
            RuleFor(x => x.EndTime).Cascade(CascadeMode.Stop).GreaterThan(x => x.StartTime).WithMessage(AppBusinessMessages.SlotEndDateInvalid);
        }

        protected override bool PreValidate(ValidationContext<SlotModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessages.SlotDetailsMissing));
                return false;
            }
            return true;
        }

    }
}
