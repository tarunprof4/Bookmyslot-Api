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
            var preProcessedSearchedCustomers = await this.searchCustomerRepository.GetPreProcessedSearchedCustomers(searchKey);
            if(preProcessedSearchedCustomers.ResultType == ResultType.Success)
            {
                return preProcessedSearchedCustomers;
            }

            var searchedCustomersResponse =  await this.searchCustomerRepository.SearchCustomers(searchKey);
            if (searchedCustomersResponse.ResultType == ResultType.Success)
            {
                await this.searchCustomerRepository.SavePreProcessedSearchedCustomers(searchKey, searchedCustomersResponse.Result);
            }

            return searchedCustomersResponse;
        }
    }
}
