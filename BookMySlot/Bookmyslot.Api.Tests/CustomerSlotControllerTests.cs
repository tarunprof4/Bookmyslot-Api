using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Encryption.Interfaces;
using Bookmyslot.Api.Common.ViewModels;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{

    public class CustomerSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string BioHeadLine = "BioHeadLine";
        private const string ProfilePic = "ProfilePic";
        private const int ValidPaseSize = 10;
        private const int InValidPaseSize = 0;
        private const int InValidPageNumber = -1;
        private const int ValidPageNumber = 0;

        private CustomerSlotController customerSlotController;
        private Mock<ICustomerSlotBusiness> customerSlotBusinessMock;
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<IDistributedInMemoryCacheBuisness> distributedInMemoryCacheBuisnessMock;
        private Mock<IHashing> hashingMock;
        private Mock<ICurrentUser> currentUserMock;
        private CacheConfiguration cacheConfiguration;


        [SetUp]
        public void Setup()
        {
            customerSlotBusinessMock = new Mock<ICustomerSlotBusiness>();
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            distributedInMemoryCacheBuisnessMock = new Mock<IDistributedInMemoryCacheBuisness>();
            hashingMock = new Mock<IHashing>();
            currentUserMock = new Mock<ICurrentUser>();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            cacheConfiguration = new CacheConfiguration(configuration);
            customerSlotController = new CustomerSlotController(customerSlotBusinessMock.Object, symmetryEncryptionMock.Object,
                distributedInMemoryCacheBuisnessMock.Object, hashingMock.Object, cacheConfiguration, currentUserMock.Object);

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_NullPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetDistinctCustomersNearestSlotFromToday(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.PaginationSettingsMissing));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_EmptyPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetDistinctCustomersNearestSlotFromToday(new PageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_InValidPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetDistinctCustomersNearestSlotFromToday(DefaultInValidPageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageNumber));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_ValidPageParameterModel_ReturnsSuccessResponse()
        {
            Response<List<CustomerSlotModel>> distributedInMemoryCacheBuisnessMockResponse = new Response<List<CustomerSlotModel>>() { Result = CreateDefaultCustomerSlotModels()};
            distributedInMemoryCacheBuisnessMock.Setup(a => a.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())).Returns(Task.FromResult(distributedInMemoryCacheBuisnessMockResponse));

            var response = await customerSlotController.GetDistinctCustomersNearestSlotFromToday(DefaultValidPageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            var customerViewModels = objectResult.Value as List<CustomerViewModel>;
            customerViewModels = customerViewModels.ToList();
            Assert.AreEqual(customerViewModels[0].FirstName, FirstName);
            Assert.AreEqual(customerViewModels[0].LastName, LastName);
            Assert.AreEqual(customerViewModels[0].BioHeadLine, BioHeadLine);
            Assert.AreEqual(customerViewModels[0].ProfilePictureUrl, ProfilePic);
            
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheModel>(), It.IsAny<Func<Task<Response<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())), Times.Once());
            hashingMock.Verify((m => m.Create(It.IsAny<string>())), Times.Once());
        }




        [Test]
        public async Task GetCustomerAvailableSlots_NullPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetCustomerAvailableSlots(null, CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.PaginationSettingsMissing));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_EmptyPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetCustomerAvailableSlots(new PageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task GetCustomerAvailableSlots_InValidPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetCustomerAvailableSlots(DefaultInValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageNumber));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_ValidPageParameterModelWithoutCustomerSettings_ReturnsSuccessResponse()
        {
            var bookAvailableSlotModel = CreateDefaultValidBookAvailableSlotModel();
            bookAvailableSlotModel.CustomerSettingsModel = null;
            Response<BookAvailableSlotModel> customerSlotBusinessMockResponse = new Response<BookAvailableSlotModel>() { Result = bookAvailableSlotModel };
            customerSlotBusinessMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customerSlotBusinessMockResponse));

            var response = await customerSlotController.GetCustomerAvailableSlots(DefaultValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.AtLeastOnce());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_ValidPageParameterModel_ReturnsSuccessResponse()
        {
            Response<BookAvailableSlotModel> customerSlotBusinessMockResponse = new Response<BookAvailableSlotModel>() { Result = CreateDefaultValidBookAvailableSlotModel() };
            customerSlotBusinessMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customerSlotBusinessMockResponse));

            var response = await customerSlotController.GetCustomerAvailableSlots(DefaultValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.AtLeastOnce());
        }


        private List<CustomerSlotModel> CreateDefaultCustomerSlotModels()
        {
            List<CustomerSlotModel> customerSlotModels = new List<CustomerSlotModel>();
            var customerSlotModel = new CustomerSlotModel() { CustomerModel = CreateDefaultCustomerModel() };
            customerSlotModels.Add(customerSlotModel);
            return customerSlotModels;
        }

        private CustomerModel CreateDefaultCustomerModel()
        {
            CustomerModel customerModel = new CustomerModel();
            customerModel.FirstName = FirstName;
            customerModel.LastName = LastName;
            customerModel.ProfilePictureUrl = ProfilePic;
            customerModel.BioHeadLine = BioHeadLine;
            return customerModel;
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
