using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.ViewModels;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{

    public class CustomerSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string BioHeadLine = "BioHeadLine";
        private const int ValidPaseSize = 10;
        private const int InValidPaseSize = 0;
        private const int InValidPageNumber = -1;
        private const int ValidPageNumber = 0;

        private CustomerSlotController customerBookedSlotController;
        private Mock<ICustomerSlotBusiness> customerSlotBusinessMock;
        private Mock<IKeyEncryptor> keyEncryptorMock;
        private Mock<IDistributedInMemoryCacheBuisness> distributedInMemoryCacheBuisnessMock;
        private Mock<IHashing> hashingMock;
        private Mock<ICurrentUser> currentUserMock;
        private CacheConfiguration cacheConfiguration;


        [SetUp]
        public void Setup()
        {
            customerSlotBusinessMock = new Mock<ICustomerSlotBusiness>();
            keyEncryptorMock = new Mock<IKeyEncryptor>();
            distributedInMemoryCacheBuisnessMock = new Mock<IDistributedInMemoryCacheBuisness>();
            hashingMock = new Mock<IHashing>();
            currentUserMock = new Mock<ICurrentUser>();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            cacheConfiguration = new CacheConfiguration(configuration);
            customerBookedSlotController = new CustomerSlotController(customerSlotBusinessMock.Object, keyEncryptorMock.Object,
                distributedInMemoryCacheBuisnessMock.Object, hashingMock.Object, cacheConfiguration, currentUserMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_NullPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerBookedSlotController.GetDistinctCustomersNearestSlotFromToday(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.PaginationSettingsMissing));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Never());
            keyEncryptorMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<object>())), Times.Never());
        }


        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_EmptyPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerBookedSlotController.GetDistinctCustomersNearestSlotFromToday(new PageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<object>())), Times.Never());
        }

        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_InValidPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerBookedSlotController.GetDistinctCustomersNearestSlotFromToday(DefaultInValidPageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageNumber));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<object>())), Times.Never());
        }


        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_ValidPageParameterModel_ReturnsSuccessResponse()
        {
            Response<List<CustomerSlotModel>> distributedInMemoryCacheBuisnessMockResponse = new Response<List<CustomerSlotModel>>() { Result = new List<CustomerSlotModel>() };
            distributedInMemoryCacheBuisnessMock.Setup(a => a.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>())).Returns(Task.FromResult(distributedInMemoryCacheBuisnessMockResponse));

            var response = await customerBookedSlotController.GetDistinctCustomersNearestSlotFromToday(DefaultValidPageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>())), Times.Once());
            hashingMock.Verify((m => m.Create(It.IsAny<object>())), Times.Once());
        }




        [Test]
        public async Task GetCustomerAvailableSlots_NullPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerBookedSlotController.GetCustomerAvailableSlots(null, CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.PaginationSettingsMissing));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            keyEncryptorMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_EmptyPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerBookedSlotController.GetCustomerAvailableSlots(new PageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            keyEncryptorMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task GetCustomerAvailableSlots_InValidPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerBookedSlotController.GetCustomerAvailableSlots(DefaultInValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageNumber));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            keyEncryptorMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_ValidPageParameterModelWithoutCustomerSettings_ReturnsSuccessResponse()
        {
            var bookAvailableSlotModel = CreateDefaultValidBookAvailableSlotModel();
            bookAvailableSlotModel.CustomerSettingsModel = null;
            Response<BookAvailableSlotModel> customerSlotBusinessMockResponse = new Response<BookAvailableSlotModel>() { Result = bookAvailableSlotModel };
            customerSlotBusinessMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customerSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerAvailableSlots(DefaultValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            keyEncryptorMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.AtLeastOnce());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_ValidPageParameterModel_ReturnsSuccessResponse()
        {
            Response<BookAvailableSlotModel> customerSlotBusinessMockResponse = new Response<BookAvailableSlotModel>() { Result = CreateDefaultValidBookAvailableSlotModel() };
            customerSlotBusinessMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customerSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerAvailableSlots(DefaultValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            keyEncryptorMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.AtLeastOnce());
        }

        private BookAvailableSlotModel CreateDefaultValidBookAvailableSlotModel()
        {
            var bookAvailableSlotModel = new BookAvailableSlotModel();
            bookAvailableSlotModel.CreatedByCustomerModel = new CustomerModel()
            {
                FirstName = FirstName,
                LastName = LastName,
                BioHeadLine = BioHeadLine
            };

            bookAvailableSlotModel.CustomerSettingsModel = new CustomerSettingsModel() { TimeZone = TimeZoneConstants.IndianTimezone };
            bookAvailableSlotModel.AvailableSlotModels = new List<SlotInforamtionInCustomerTimeZoneModel>();
            bookAvailableSlotModel.AvailableSlotModels.Add(new SlotInforamtionInCustomerTimeZoneModel() { SlotModel = new SlotModel(), CustomerSlotZonedDateTime = new ZonedDateTime() });

            return bookAvailableSlotModel;
        }

        private PageParameterViewModel DefaultInValidPageParameterViewModel()
        {
            return new PageParameterViewModel() { PageNumber = InValidPageNumber, PageSize = InValidPaseSize };
        }

        private PageParameterViewModel DefaultValidPageParameterViewModel()
        {
            return new PageParameterViewModel() { PageNumber = ValidPageNumber, PageSize = ValidPaseSize };
        }

    }
}
