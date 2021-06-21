using Bookmyslot.Api.Common.Contracts;
using Nest;

namespace Bookmyslot.Api.Common.Search.Contracts
{
    public class SearchCustomerModel
    {
        [Keyword]
        public string Id { get; set; }


        [Keyword]
        public string Email { get; set; }

        [Keyword]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        [Text]
        public string BioHeadLine { get; set; }


        public static string GetSearchCustomerCacehKey(string searchKey, PageParameterModel pageParameterModel)
        {
            return string.Format("{0}-{1}-{2}", searchKey, pageParameterModel.PageNumber, pageParameterModel.PageSize);
        }
    }
}
