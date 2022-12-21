using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Constants.cs;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.ValueObject;
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
        public async Task<Result<List<SearchCustomerModel>>> SearchCustomers(string searchKey, PageParameterModel pageParameterModel)
        {
            searchKey = searchKey.ToLowerInvariant();
            var cacheSearchKey = CacheModel.GetSearchCustomerCacehKey(searchKey, pageParameterModel);

            var preProcessedSearchedCustomers = await this.searchRepository.GetPreProcessedSearchedResponse<List<SearchCustomerModel>>(SearchConstants.SearchCustomer, cacheSearchKey);
            if (preProcessedSearchedCustomers.ResultType == ResultType.Success)
            {
                return preProcessedSearchedCustomers;
            }

            var searchedCustomersResponse = await this.SearchCustomersBySearchIntent(searchKey, pageParameterModel);
            if (searchedCustomersResponse.ResultType == ResultType.Success)
            {
                await this.searchRepository.SavePreProcessedSearchedResponse(SearchConstants.SearchCustomer, cacheSearchKey, searchedCustomersResponse.Value);
            }

            return searchedCustomersResponse;
        }


        public async Task<Result<List<SearchCustomerModel>>> SearchCustomersBySearchIntent(string searchKey, PageParameterModel pageParameterModel)
        {
            if (searchKey[0] == SearchConstants.SearchCustomerByNameIdentifier)
            {
                return await this.searchCustomerRepository.SearchCustomersByName(SanitizeSearchKey(searchKey), pageParameterModel);
            }
            else if (searchKey[0] == SearchConstants.SearchCustomerByUserNameIdentifier)
            {
                var searchByUserNameResponse = await this.searchCustomerRepository.SearchCustomersByUserName(SanitizeSearchKey(searchKey));
                if (searchByUserNameResponse.ResultType == ResultType.Success)
                {
                    var searchedCustomerModelResponse = new Result<List<SearchCustomerModel>>
                    {
                        Value = new List<SearchCustomerModel>() { searchByUserNameResponse.Value }
                    };

                    return searchedCustomerModelResponse;
                }

                return new Result<List<SearchCustomerModel>>()
                { ResultType = searchByUserNameResponse.ResultType, Messages = searchByUserNameResponse.Messages };
            }

            else
            {
                return await this.searchCustomerRepository.SearchCustomersByBioHeadLine(searchKey, pageParameterModel);
            }
        }


        private string SanitizeSearchKey(string searchKey)
        {
            return searchKey.Substring(1, searchKey.Length - 1);
        }


    }
}
