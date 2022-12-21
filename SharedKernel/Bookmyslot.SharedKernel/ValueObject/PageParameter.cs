
using Bookmyslot.SharedKernel.Constants;

namespace Bookmyslot.SharedKernel.ValueObject
{
    public class PageParameter
    {
        const int maxPageSize = PaginationConstants.PageSize;
        private int pageNumber;
        private int pageSize;
        public PageParameter(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber;
            this.pageSize = (pageSize > maxPageSize) ? maxPageSize : pageSize; ;
        }
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
        }

        public int PageSize
        {
            get
            {
                return pageSize;
            }
        }
    }
}
