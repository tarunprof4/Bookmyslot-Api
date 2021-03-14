using Bookmyslot.Api.Common.Contracts.Constants;
using FluentValidation;
using FluentValidation.Results;

namespace Bookmyslot.Api.Common.ViewModels.Validations
{
    public class PageParameterViewModelValidator : AbstractValidator<PageParameterViewModel>
    {
        public PageParameterViewModelValidator()
        {
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
