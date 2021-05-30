using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Constants.cs;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.ViewModels;
using Bookmyslot.Api.Common.ViewModels.Validations;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.Api.Web.Common;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    //[Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    public class CustomerSlotController : BaseApiController
    {
        private readonly ICustomerSlotBusiness customerSlotBusiness;
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness;
        private readonly IHashing sha256SaltedHash;
        private readonly CacheConfiguration cacheConfiguration;
        private readonly ICurrentUser currentUser;
        private readonly ICustomerResponseAdaptor customerResponseAdaptor;
        private readonly IAvailableSlotResponseAdaptor availableSlotResponseAdaptor;
        

        public CustomerSlotController(ICustomerSlotBusiness customerSlotBusiness, ISymmetryEncryption symmetryEncryption, IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness, IHashing sha256SaltedHash, CacheConfiguration cacheConfiguration, ICurrentUser currentUser, ICustomerResponseAdaptor customerResponseAdaptor, IAvailableSlotResponseAdaptor availableSlotResponseAdaptor)
        {
            this.customerSlotBusiness = customerSlotBusiness;
            this.symmetryEncryption = symmetryEncryption;
            this.distributedInMemoryCacheBuisness = distributedInMemoryCacheBuisness;
            this.sha256SaltedHash = sha256SaltedHash;
            this.cacheConfiguration = cacheConfiguration;
            this.currentUser = currentUser;
            this.customerResponseAdaptor = customerResponseAdaptor;
            this.availableSlotResponseAdaptor = availableSlotResponseAdaptor;
        }

        /// <summary>
        /// Gets customer slots
        /// </summary>
        /// <param name="pageParameterViewModel">pageParameterModel</param>
        /// <returns>returns slot model</returns>
        /// <response code="200">Returns customer slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        // GET: api/<CustomerSlot>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerSlot/GetDistinctCustomersNearestSlotFromToday")]
        [HttpGet()]
        [ActionName("GetHomePageSlots")]
        public async Task<IActionResult> GetDistinctCustomersNearestSlotFromToday([FromQuery] PageParameterViewModel pageParameterViewModel)
        {
            var validator = new PageParameterViewModelValidator();
            ValidationResult results = validator.Validate(pageParameterViewModel);

            if (results.IsValid)
            {
                var pageParameterModel = CreatePageParameterModel(pageParameterViewModel);
                var cacheModel = CreateCacheModel(pageParameterModel);

                var customerSlotModels = await this.distributedInMemoryCacheBuisness.GetFromCacheAsync(
                          cacheModel,
                          () => this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel));


                if (customerSlotModels.ResultType == ResultType.Success)
                {
                    var customerViewModelResponse = new Response<IEnumerable<CustomerViewModel>>()
                    { Result = this.customerResponseAdaptor.CreateCustomerViewModels(customerSlotModels.Result) };
                    return this.CreateGetHttpResponse(customerViewModelResponse);

                }
                return this.CreateGetHttpResponse(new Response<IEnumerable<CustomerViewModel>>()
                {
                    ResultType = customerSlotModels.ResultType,
                    Messages = customerSlotModels.Messages
                });
            }

            var validationResponse = Response<CustomerSlotModel>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreateGetHttpResponse(validationResponse);
        }

        private CacheModel CreateCacheModel(PageParameterModel pageParameterModel)
        {
            var cacheModel = new CacheModel();
            var sha256SaltedHashKey = this.sha256SaltedHash.Create(JsonConvert.SerializeObject(pageParameterModel));
            cacheModel.Key = string.Format(CacheConstants.GetDistinctCustomersNearestSlotFromTodayCacheKey, sha256SaltedHashKey);

            cacheModel.ExpiryTime = TimeSpan.FromSeconds(this.cacheConfiguration.HomePageInSeconds);
            return cacheModel;
        }


        /// <summary>
        /// Gets customer slots
        /// </summary>
        /// <param name="pageParameterViewModel">pageParameterModel</param>
        /// <param name="customerInfo">customerInfo</param>
        /// <returns>returns slot model</returns>
        /// <response code="200">Returns customer slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        // GET: api/<CustomerSlot>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerSlot/GetCustomerAvailableSlots")]
        [HttpGet()]
        [ActionName("GetCustomerAvailableSlots")]
        public async Task<IActionResult> GetCustomerAvailableSlots([FromQuery] PageParameterViewModel pageParameterViewModel, string customerInfo)
        {
            var validator = new PageParameterViewModelValidator();
            ValidationResult results = validator.Validate(pageParameterViewModel);

            if (results.IsValid)
            {
                var createdByUser = this.symmetryEncryption.Decrypt(customerInfo);

                if (createdByUser != string.Empty)
                {
                    var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                    var customerId = currentUserResponse.Result.Id;

                    var pageParameterModel = CreatePageParameterModel(pageParameterViewModel);
                    var bookAvailableSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, customerId, createdByUser);

                    var bookAvailableSlotViewModelResponse = this.availableSlotResponseAdaptor.CreateBookAvailableSlotViewModel(bookAvailableSlotModelResponse);
                    return this.CreateGetHttpResponse(bookAvailableSlotViewModelResponse);
                }

                var validationErrorResponse = Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CorruptData });
                return this.CreateGetHttpResponse(validationErrorResponse);
            }

            var validationResponse = Response<BookAvailableSlotViewModel>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreateGetHttpResponse(validationResponse);
        }

        private PageParameterModel CreatePageParameterModel(PageParameterViewModel pageParameterViewModel)
        {
            return new PageParameterModel() { PageNumber = pageParameterViewModel.PageNumber, PageSize = pageParameterViewModel.PageSize };
        }
    }
}
