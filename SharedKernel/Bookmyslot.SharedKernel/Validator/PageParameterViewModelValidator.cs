using Bookmyslot.SharedKernel.Constants;
using Bookmyslot.SharedKernel.ValueObject;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.SharedKernel.Validator
{
    public class PageParameterViewModelValidator : AbstractValidator<PageParameterViewModel>
    {
        public PageParameterViewModelValidator()
        {
            RuleFor(x => x.PageNumber).Cascade(CascadeMode.Stop).Must(isPageNumberValid).WithMessage(AppBusinessMessagesConstants.InValidPageNumber);
            RuleFor(x => x.PageSize).Cascade(CascadeMode.Stop).Must(isPageSizeValid).WithMessage(AppBusinessMessagesConstants.InValidPageSize);
        }

        protected override bool PreValidate(ValidationContext<PageParameterViewModel> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AppBusinessMessagesConstants.PaginationSettingsMissing));
                return false;
            }
            return true;
        }

        private bool isPageNumberValid(int pageNumber)
        {
            if (pageNumber >= 0)
            {
                return true;
            }
            return false;
        }

        private bool isPageSizeValid(int pageSize)
        {
            if (pageSize == 0)
            {
                return false;
            }
            return true;
        }
    }
}
