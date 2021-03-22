using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Constants.cs;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.ViewModels;
using Bookmyslot.Api.Common.ViewModels.Validations;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
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
        private readonly IKeyEncryptor keyEncryptor;
        private readonly IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness;
        private readonly IHashing md5Hash;
        private readonly CacheConfiguration cacheConfiguration;
        private readonly ICurrentUser currentUser;


        public CustomerSlotController(ICustomerSlotBusiness customerSlotBusiness, IKeyEncryptor keyEncryptor, IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness, IHashing md5Hash, CacheConfiguration cacheConfiguration, ICurrentUser currentUser)
        {
            this.customerSlotBusiness = customerSlotBusiness;
            this.keyEncryptor = keyEncryptor;
            this.distributedInMemoryCacheBuisness = distributedInMemoryCacheBuisness;
            this.md5Hash = md5Hash;
            this.cacheConfiguration = cacheConfiguration;
            this.currentUser = currentUser;
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

                var customerSlotModels =
                      await
                      this.distributedInMemoryCacheBuisness.GetFromCacheAsync(
                          cacheModel,
                          () => this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel));


                if (customerSlotModels.ResultType == ResultType.Success)
                {
                    HideUncessaryDetailsForGetDistinctCustomersNearestSlotFromToday(customerSlotModels.Result);
                }
                return this.CreateGetHttpResponse(customerSlotModels);
            }

            var validationResponse = Response<CustomerSlotModel>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreateGetHttpResponse(validationResponse);
        }

        private CacheModel CreateCacheModel(PageParameterModel pageParameterModel)
        {
            var cacheModel = new CacheModel();
            var md5HashKey = this.md5Hash.Create(pageParameterModel);
            cacheModel.Key = string.Format(CacheConstants.GetDistinctCustomersNearestSlotFromTodayCacheKey, md5HashKey);

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
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result;

                var pageParameterModel = CreatePageParameterModel(pageParameterViewModel);
                var bookAvailableSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, customerId, customerInfo);

                var bookAvailableSlotViewModelResponse = CreateBookAvailableSlotViewModel(bookAvailableSlotModelResponse);
                return this.CreateGetHttpResponse(bookAvailableSlotViewModelResponse);
            }

            var validationResponse = Response<BookAvailableSlotViewModel>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreateGetHttpResponse(validationResponse);
        }

        private void HideUncessaryDetailsForGetDistinctCustomersNearestSlotFromToday(List<CustomerSlotModel> customerSlotModels)
        {
            foreach (var customerSlotModel in customerSlotModels)
            {
                customerSlotModel.SlotModels = new List<SlotModel>();
            }
        }


        private Response<BookAvailableSlotViewModel> CreateBookAvailableSlotViewModel(Response<BookAvailableSlotModel> bookAvailableSlotModelResponse)
        {
            if (bookAvailableSlotModelResponse.ResultType == ResultType.Success)
            {
                var bookAvailableSlotModel = bookAvailableSlotModelResponse.Result;
                var bookAvailableSlotViewModel = new BookAvailableSlotViewModel
                {
                    CreatedByCustomerViewModel = new CustomerViewModel(bookAvailableSlotModel.CreatedByCustomerModel.FirstName,
                    bookAvailableSlotModel.CreatedByCustomerModel.LastName, bookAvailableSlotModel.CreatedByCustomerModel.BioHeadLine),
                    ToBeBookedByCustomerCountry = bookAvailableSlotModel.CustomerSettingsModel != null ? bookAvailableSlotModel.CustomerSettingsModel.Country : string.Empty,
                    BookAvailableSlotModels = new List<SlotInformationInCustomerTimeZoneViewModel>()
                };

                foreach (var availableSlotModel in bookAvailableSlotModel.AvailableSlotModels)
                {
                    var slotInformation = this.keyEncryptor.Encrypt(JsonConvert.SerializeObject(availableSlotModel.SlotModel));
                    bookAvailableSlotViewModel.BookAvailableSlotModels.Add(new SlotInformationInCustomerTimeZoneViewModel()
                    {
                        Title = availableSlotModel.SlotModel.Title,
                        Country = availableSlotModel.SlotModel.Country,
                        SlotDuration = availableSlotModel.SlotModel.SlotDuration,
                        SlotStartZonedDateTime = availableSlotModel.SlotModel.SlotStartZonedDateTime,
                        CustomerSlotStartZonedDateTime = availableSlotModel.CustomerSlotZonedDateTime,
                        SlotInformation = slotInformation
                    });
                }

                return new Response<BookAvailableSlotViewModel>() { Result = bookAvailableSlotViewModel };
            }

            return new Response<BookAvailableSlotViewModel>()
            {
                ResultType = bookAvailableSlotModelResponse.ResultType,
                Messages = bookAvailableSlotModelResponse.Messages
            };
        }

        private PageParameterModel CreatePageParameterModel(PageParameterViewModel pageParameterViewModel)
        {
            return new PageParameterModel() { PageNumber = pageParameterViewModel.PageNumber, PageSize = pageParameterViewModel.PageSize };
        }
    }
}
