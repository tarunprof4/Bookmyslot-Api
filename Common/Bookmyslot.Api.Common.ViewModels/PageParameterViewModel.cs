using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Common.ViewModels
{
    public class PageParameterViewModel
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
