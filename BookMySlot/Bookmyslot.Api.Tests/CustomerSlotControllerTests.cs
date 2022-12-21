using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Cache.Contracts.Configuration;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.SharedKernel.Constants;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.ValueObject;
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
        private Mock<ICustomerResponseAdaptor> customerResponseAdaptorMock;
        private Mock<IAvailableSlotResponseAdaptor> availableSlotResponseAdaptorMock;

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
            customerResponseAdaptorMock = new Mock<ICustomerResponseAdaptor>();
            availableSlotResponseAdaptorMock = new Mock<IAvailableSlotResponseAdaptor>();

            customerSlotController = new CustomerSlotController(customerSlotBusinessMock.Object, symmetryEncryptionMock.Object,
                distributedInMemoryCacheBuisnessMock.Object, hashingMock.Object, cacheConfiguration, currentUserMock.Object,
                customerResponseAdaptorMock.Object, availableSlotResponseAdaptorMock.Object);

            Result<CurrentUserModel> currentUserMockResponse = new Result<CurrentUserModel>() { Value = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_NullPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetDistinctCustomersNearestSlotFromToday(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(SharedKernel.Constants.AppBusinessMessagesConstants.PaginationSettingsMissing));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameter>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheKeyExpiry>(), It.IsAny<Func<Task<Result<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_EmptyPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetDistinctCustomersNearestSlotFromToday(new PageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(SharedKernel.Constants.AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameter>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheKeyExpiry>(), It.IsAny<Func<Task<Result<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_InValidPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetDistinctCustomersNearestSlotFromToday(DefaultInValidPageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(SharedKernel.Constants.AppBusinessMessagesConstants.InValidPageNumber));
            Assert.IsTrue(validationMessages.Contains(SharedKernel.Constants.AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameter>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheKeyExpiry>(), It.IsAny<Func<Task<Result<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())), Times.Never());
            hashingMock.Verify((m => m.Create(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetDistinctCustomersNearestSlotFromToday_ValidPageParameterModel_ReturnsSuccessResponse()
        {
            Result<List<CustomerSlotModel>> distributedInMemoryCacheBuisnessMockResponse = new Result<List<CustomerSlotModel>>() { Value = CreateDefaultCustomerSlotModels() };
            distributedInMemoryCacheBuisnessMock.Setup(a => a.GetFromCacheAsync(It.IsAny<CacheKeyExpiry>(), It.IsAny<Func<Task<Result<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())).Returns(Task.FromResult(distributedInMemoryCacheBuisnessMockResponse));
            IEnumerable<CustomerViewModel> createCustomerViewModelsMockResponse = new List<CustomerViewModel>();
            customerResponseAdaptorMock.Setup(a => a.CreateCustomerViewModels(It.IsAny<IEnumerable<CustomerModel>>())).Returns(createCustomerViewModelsMockResponse);

            var response = await customerSlotController.GetDistinctCustomersNearestSlotFromToday(DefaultValidPageParameterViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            var customerViewModels = objectResult.Value as List<CustomerViewModel>;
            customerSlotBusinessMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameter>())), Times.Never());
            distributedInMemoryCacheBuisnessMock.Verify((m => m.GetFromCacheAsync(It.IsAny<CacheKeyExpiry>(), It.IsAny<Func<Task<Result<List<CustomerSlotModel>>>>>(), It.IsAny<bool>())), Times.Once());
            hashingMock.Verify((m => m.Create(It.IsAny<string>())), Times.Once());
        }




        [Test]
        public async Task GetCustomerAvailableSlots_NullPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetCustomerAvailableSlots(null, CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(SharedKernel.Constants.AppBusinessMessagesConstants.PaginationSettingsMissing));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_EmptyPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetCustomerAvailableSlots(new PageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(SharedKernel.Constants.AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task GetCustomerAvailableSlots_InValidPageParameterModel_ReturnsValidationResponse()
        {
            var response = await customerSlotController.GetCustomerAvailableSlots(DefaultInValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(SharedKernel.Constants.AppBusinessMessagesConstants.InValidPageNumber));
            Assert.IsTrue(validationMessages.Contains(SharedKernel.Constants.AppBusinessMessagesConstants.InValidPageSize));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Never());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_InValidCustomerInfoEncryptedKey_ReturnsValidationResponse()
        {
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var response = await customerSlotController.GetCustomerAvailableSlots(DefaultValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(Common.Contracts.Constants.AppBusinessMessagesConstants.CorruptData));
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Encrypt(It.IsAny<string>())), Times.Never());
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerAvailableSlots_ValidPageParameterModelWithoutCustomerSettings_ReturnsSuccessResponse()
        {
            var bookAvailableSlotModel = CreateDefaultValidBookAvailableSlotModel();
            bookAvailableSlotModel.CustomerSettingsModel = null;
            Result<BookAvailableSlotModel> customerSlotBusinessMockResponse = new Result<BookAvailableSlotModel>() { Value = bookAvailableSlotModel };
            customerSlotBusinessMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customerSlotBusinessMockResponse));
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(CustomerId);
            Result<BookAvailableSlotViewModel> availableSlotResponseAdaptorMockResponse = new Result<BookAvailableSlotViewModel>() { Value = CreateDefaultValidBookAvailableSlotViewModel() };
            availableSlotResponseAdaptorMock.Setup(a => a.CreateBookAvailableSlotViewModel(It.IsAny<Result<BookAvailableSlotModel>>())).Returns(availableSlotResponseAdaptorMockResponse);


            var response = await customerSlotController.GetCustomerAvailableSlots(DefaultValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            availableSlotResponseAdaptorMock.Verify((m => m.CreateBookAvailableSlotViewModel(It.IsAny<Result<BookAvailableSlotModel>>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_ValidPageParameterModel_ReturnsSuccessResponse()
        {
            Result<BookAvailableSlotModel> customerSlotBusinessMockResponse = new Result<BookAvailableSlotModel>() { Value = CreateDefaultValidBookAvailableSlotModel() };
            customerSlotBusinessMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customerSlotBusinessMockResponse));
            symmetryEncryptionMock.Setup(a => a.Decrypt(It.IsAny<string>())).Returns(CustomerId);
            Result<BookAvailableSlotViewModel> availableSlotResponseAdaptorMockResponse = new Result<BookAvailableSlotViewModel>() { Value = CreateDefaultValidBookAvailableSlotViewModel() };
            availableSlotResponseAdaptorMock.Setup(a => a.CreateBookAvailableSlotViewModel(It.IsAny<Result<BookAvailableSlotModel>>())).Returns(availableSlotResponseAdaptorMockResponse);

            var response = await customerSlotController.GetCustomerAvailableSlots(DefaultValidPageParameterViewModel(), CustomerId);

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            customerSlotBusinessMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameter>(), It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            symmetryEncryptionMock.Verify((m => m.Decrypt(It.IsAny<string>())), Times.Once());
            availableSlotResponseAdaptorMock.Verify((m => m.CreateBookAvailableSlotViewModel(It.IsAny<Result<BookAvailableSlotModel>>())), Times.Once());
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

        private BookAvailableSlotViewModel CreateDefaultValidBookAvailableSlotViewModel()
        {
            return new BookAvailableSlotViewModel();
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
