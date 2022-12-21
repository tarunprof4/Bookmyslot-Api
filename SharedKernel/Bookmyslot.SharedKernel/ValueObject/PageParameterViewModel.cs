using Bookmyslot.SharedKernel.Constants;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.SharedKernel.ValueObject
{
    public class PageParameterViewModel : BaseValueObject
    {
        [Required]
        [DefaultValue("0")]
        public int PageNumber { get; set; }

        const int maxPageSize = PaginationConstants.PageSize;

        private int pageSize;

        [DefaultValue("100")]
        [Range(1, PaginationConstants.PageSize)]
        public int PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}