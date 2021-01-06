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

        [JsonIgnore]
        [DefaultValue("10")]
        public int PageSize
        {
            get
            {
                return PaginationConstants.PageSize;
            }
        }
    }
}
