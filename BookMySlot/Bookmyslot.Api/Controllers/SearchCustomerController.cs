using Bookmyslot.Api.Cache.Contracts.Configuration;
using Bookmyslot.Api.Cache.Contracts.Constants.cs;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Web.Filters;
using Bookmyslot.Api.Search.Contracts;
using Bookmyslot.Api.Search.Contracts.Constants.cs;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(AuthorizedFilter))]
    public class SearchCustomerController : BaseApiController
    {
        private readonly ISearchCustomerBusiness searchCustomerBusiness;
        private readonly IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness;
        private readonly CacheConfiguration cacheConfiguration;

        public SearchCustomerController(ISearchCustomerBusiness searchCustomerBusiness, IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness, CacheConfiguration cacheConfiguration)
        {
            this.searchCustomerBusiness = searchCustomerBusiness;
            this.distributedInMemoryCacheBuisness = distributedInMemoryCacheBuisness;
            this.cacheConfiguration = cacheConfiguration;
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{searchKey}")]
        [ActionName("SearchCustomer")]
        public async Task<IActionResult> Get(string searchKey, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                var validationResponse = Result<List<SearchCustomerModel>>.ValidationError(new List<string>() { AppBusinessMessagesConstants.InValidSearchKey });
                return this.CreateGetHttpResponse(validationResponse);
            }

            if (SearchConstants.SearchCustomerMinKeyLength > searchKey.Length - 1)
            {
                var validationResponse = Result<List<SearchCustomerModel>>.ValidationError(new List<string>() { AppBusinessMessagesConstants.InValidCustomerSearchKeyMinLength });
                return this.CreateGetHttpResponse(validationResponse);
            }

            var pageParameterModel = new PageParameter(pageNumber, pageSize);

            var cacheModel = CreateCacheModel(searchKey, pageParameterModel);

            var customerResponse = await
                  this.distributedInMemoryCacheBuisness.GetFromCacheAsync(cacheModel,
                  () => this.searchCustomerBusiness.SearchCustomers(searchKey, pageParameterModel));

            return this.CreateGetHttpResponse(customerResponse);
        }

        private CacheKeyExpiry CreateCacheModel(string searchKey, PageParameter pageParameterModel)
        {
            var cacheModel = new CacheKeyExpiry();
            var cacheSearchKey = CacheKeyExpiry.GetSearchCustomerCacehKey(searchKey, pageParameterModel);
            cacheModel.Key = string.Format(CacheConstants.CustomerSearchKey, cacheSearchKey);
            cacheModel.ExpiryTime = TimeSpan.FromSeconds(this.cacheConfiguration.CustomerSearchInSeconds);
            return cacheModel;
        }
    }
}

