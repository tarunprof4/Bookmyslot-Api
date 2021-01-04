using Bookmyslot.Api.Common.Contracts.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookmyslot.Api.Common.Contracts
{
    public class PageParameterModel
    {
        [Required]
        public int PageNumber { get; set; }

        [JsonIgnore]
        public int PageSize
        {
            get
            {
                return PaginationConstants.PageSize;
            }
        }
    }
}
