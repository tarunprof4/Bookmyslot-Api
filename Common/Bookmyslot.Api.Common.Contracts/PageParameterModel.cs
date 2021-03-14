using Bookmyslot.Api.Common.Contracts.Constants;

namespace Bookmyslot.Api.Common.Contracts
{
    public class PageParameterModel
    {
        public int PageNumber { get; set; }

        const int maxPageSize = PaginationConstants.PageSize;

        private int pageSize;

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
