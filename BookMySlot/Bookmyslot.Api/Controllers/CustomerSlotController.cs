using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Cache.Contracts.Configuration;
using Bookmyslot.Api.Cache.Contracts.Constants.cs;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Web.Filters;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.Api.Web.Common;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.Validator;
using Bookmyslot.SharedKernel.ValueObject;
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
    [ServiceFilter(typeof(AuthorizedFilter))]
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
                    var customerViewModelResponse = new Result<IEnumerable<CustomerViewModel>>()
                    { Value = this.customerResponseAdaptor.CreateCustomerViewModels(customerSlotModels.Value) };
                    return this.CreateGetHttpResponse(customerViewModelResponse);

                }
                return this.CreateGetHttpResponse(new Result<IEnumerable<CustomerViewModel>>()
                {
                    ResultType = customerSlotModels.ResultType,
                    Messages = customerSlotModels.Messages
                });
            }

            var validationResponse = Result<CustomerSlotModel>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreateGetHttpResponse(validationResponse);
        }

        private CacheKeyExpiry CreateCacheModel(PageParameter pageParameterModel)
        {
            var cacheModel = new CacheKeyExpiry();
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
                    var customerId = currentUserResponse.Value.Id;

                    var pageParameterModel = CreatePageParameterModel(pageParameterViewModel);
                    var bookAvailableSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, customerId, createdByUser);

                    var bookAvailableSlotViewModelResponse = this.availableSlotResponseAdaptor.CreateBookAvailableSlotViewModel(bookAvailableSlotModelResponse);
                    return this.CreateGetHttpResponse(bookAvailableSlotViewModelResponse);
                }

                var validationErrorResponse = Result<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CorruptData });
                return this.CreateGetHttpResponse(validationErrorResponse);
            }

            var validationResponse = Result<BookAvailableSlotViewModel>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreateGetHttpResponse(validationResponse);
        }

        private PageParameter CreatePageParameterModel(PageParameterViewModel pageParameterViewModel)
        {
            return new PageParameter(pageParameterViewModel.PageNumber, pageParameterViewModel.PageSize);
        }
    }
}
