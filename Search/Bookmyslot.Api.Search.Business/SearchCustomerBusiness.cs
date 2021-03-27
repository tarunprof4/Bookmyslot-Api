using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Constants.cs;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Business
{
    public class SearchCustomerBusiness : ISearchCustomerBusiness
    {
        private readonly ISearchRepository searchRepository;
        private readonly ISearchCustomerRepository searchCustomerRepository;
        public SearchCustomerBusiness(ISearchRepository searchRepository, ISearchCustomerRepository searchCustomerRepository)
        {
            this.searchRepository = searchRepository;
            this.searchCustomerRepository = searchCustomerRepository;
        }
        public async Task<Response<List<SearchCustomerModel>>> SearchCustomers(string searchKey)
        {
            var preProcessedSearchedCustomers = await this.searchRepository.GetPreProcessedSearchedResponse<List<SearchCustomerModel>>(SearchConstants.SearchCustomer, searchKey);
            if (preProcessedSearchedCustomers.ResultType == ResultType.Success)
            {
                return preProcessedSearchedCustomers;
            }

            var searchedCustomersResponse = await this.SearchCustomersBySearchIntent(searchKey);
            if (searchedCustomersResponse.ResultType == ResultType.Success)
            {
                await this.searchRepository.SavePreProcessedSearchedResponse(SearchConstants.SearchCustomer, searchKey, searchedCustomersResponse.Result);
            }

            return searchedCustomersResponse;
        }


        public async Task<Response<List<SearchCustomerModel>>> SearchCustomersBySearchIntent(string searchKey)
        {
            if (searchKey[0] == SearchConstants.SearchCustomerByNameIdentifier)
            {
                return await this.searchCustomerRepository.SearchCustomersByName(SanitizeSearchKey(searchKey));
            }
            else if (searchKey[0] == SearchConstants.SearchCustomerByUserNameIdentifier)
            {
                var searchByUserNameResponse = await this.searchCustomerRepository.SearchCustomersByUserName(SanitizeSearchKey(searchKey));
                if (searchByUserNameResponse.ResultType == ResultType.Success)
                {
                    var searchedCustomerModelResponse = new Response<List<SearchCustomerModel>>
                    {
                        Result = new List<SearchCustomerModel>() { searchByUserNameResponse.Result }
                    };

                    return searchedCustomerModelResponse;
                }
            }

            return await this.searchCustomerRepository.SearchCustomersByBioHeadLine(searchKey);
        }


        private string SanitizeSearchKey(string searchKey)
        {
            return searchKey.Substring(1, searchKey.Length - 1);
        }


    }
}
