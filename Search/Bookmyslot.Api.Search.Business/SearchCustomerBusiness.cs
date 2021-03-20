using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Business
{
    public class SearchCustomerBusiness : ISearchCustomerBusiness
    {
        private readonly ISearchCustomerRepository searchCustomerRepository;
        public SearchCustomerBusiness(ISearchCustomerRepository searchCustomerRepository)
        {
            this.searchCustomerRepository = searchCustomerRepository;
        }
        public async Task<Response<List<SearchCustomerModel>>> SearchCustomers(string searchKey)
        {
            var searchCustomersResponse = await this.searchCustomerRepository.GetPreProcessedSearchedCustomers(searchKey);
            if(searchCustomersResponse.ResultType == ResultType.Success)
            {
                return searchCustomersResponse;
            }

            var searchCustomerResponse =  await this.searchCustomerRepository.SearchCustomers(searchKey);
            if (searchCustomersResponse.ResultType == ResultType.Success)
            {
                await this.searchCustomerRepository.SavePreProcessedSearchedCustomers(searchKey, searchCustomerResponse.Result);
            }

            return searchCustomersResponse;
        }
    }
}
