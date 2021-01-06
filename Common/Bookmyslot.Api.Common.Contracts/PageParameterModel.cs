using Bookmyslot.Api.Common.Contracts.Constants;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Common.Contracts
{
    public class PageParameterModel
    {
        [Required]
        [DefaultValue("0")]
        public int PageNumber { get; set; }

        const int maxPageSize = PaginationConstants.PageSize;

        private int pageSize;

        [JsonIgnore]
        [DefaultValue("10")]
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
