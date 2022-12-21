using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Constants;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NodaTime;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class CustomerBookedSlotControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string BioHeadLine = "BioHeadLine";
        private const string IndianTimezone = TimeZoneConstants.IndianTimezone;
        private CustomerBookedSlotController customerBookedSlotController;
        private Mock<ICustomerBookedSlotBusiness> customerBookedSlotBusinessMock;
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<ICurrentUser> currentUserMock;
        private Mock<ICustomerResponseAdaptor> customerResponseAdaptorMock;
        private Mock<ICancelledSlotResponseAdaptor> cancelledSlotResponseAdaptorMock;
        private Mock<IBookedSlotResponseAdaptor> bookedSlotResponseAdaptorMock;


        [SetUp]
        public void Setup()
        {
            customerBookedSlotBusinessMock = new Mock<ICustomerBookedSlotBusiness>();
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            currentUserMock = new Mock<ICurrentUser>();
            customerResponseAdaptorMock = new Mock<ICustomerResponseAdaptor>();
            cancelledSlotResponseAdaptorMock = new Mock<ICancelledSlotResponseAdaptor>();
            bookedSlotResponseAdaptorMock = new Mock<IBookedSlotResponseAdaptor>();
            customerResponseAdaptorMock = new Mock<ICustomerResponseAdaptor>();
            cancelledSlotResponseAdaptorMock = new Mock<ICancelledSlotResponseAdaptor>();
            bookedSlotResponseAdaptorMock = new Mock<IBookedSlotResponseAdaptor>();

            customerBookedSlotController = new CustomerBookedSlotController(customerBookedSlotBusinessMock.Object,
                symmetryEncryptionMock.Object, currentUserMock.Object, customerResponseAdaptorMock.Object,
                cancelledSlotResponseAdaptorMock.Object, bookedSlotResponseAdaptorMock.Object);

            Result<CurrentUserModel> currentUserMockResponse = new Result<CurrentUserModel>() { Value = new CurrentUserModel() { Id = CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetCustomerBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Result<BookedSlotModel> customerBookedSlotBusinessMockResponse = new Result<BookedSlotModel>() { ResultType = ResultType.Empty };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));
            Result<BookedSlotViewModel> bookedSlotResponseAdaptorMockResponse = new Result<BookedSlotViewModel>() { ResultType = ResultType.Empty };
            bookedSlotResponseAdaptorMock.Setup(a => a.CreateBookedSlotViewModel(It.IsAny<Result<BookedSlotModel>>())).Returns(bookedSlotResponseAdaptorMockResponse);

            var response = await customerBookedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            bookedSlotResponseAdaptorMock.Verify((m => m.CreateBookedSlotViewModel(It.IsAny<Result<BookedSlotModel>>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerBookedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Result<BookedSlotModel> customerBookedSlotBusinessMockResponse = new Result<BookedSlotModel>() { Value = CreateDefaultBookedSlotModel() };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));
            Result<BookedSlotViewModel> bookedSlotResponseAdaptorMockResponse = new Result<BookedSlotViewModel>() { Value = CreateDefaultBookedSlotViewModel() };
            bookedSlotResponseAdaptorMock.Setup(a => a.CreateBookedSlotViewModel(It.IsAny<Result<BookedSlotModel>>())).Returns(bookedSlotResponseAdaptorMockResponse);

            var response = await customerBookedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            bookedSlotResponseAdaptorMock.Verify((m => m.CreateBookedSlotViewModel(It.IsAny<Result<BookedSlotModel>>())), Times.Once());
        }



        [Test]
        public async Task GetCustomerCompletedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Result<BookedSlotModel> customerBookedSlotBusinessMockResponse = new Result<BookedSlotModel>() { ResultType = ResultType.Empty };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));
            Result<BookedSlotViewModel> bookedSlotResponseAdaptorMockResponse = new Result<BookedSlotViewModel>() { ResultType = ResultType.Empty };
            bookedSlotResponseAdaptorMock.Setup(a => a.CreateBookedSlotViewModel(It.IsAny<Result<BookedSlotModel>>())).Returns(bookedSlotResponseAdaptorMockResponse);


            var response = await customerBookedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            bookedSlotResponseAdaptorMock.Verify((m => m.CreateBookedSlotViewModel(It.IsAny<Result<BookedSlotModel>>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerCompletedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Result<BookedSlotModel> customerBookedSlotBusinessMockResponse = new Result<BookedSlotModel>() { Value = CreateDefaultBookedSlotModel() };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));
            Result<BookedSlotViewModel> bookedSlotResponseAdaptorMockResponse = new Result<BookedSlotViewModel>() { Value = CreateDefaultBookedSlotViewModel() };
            bookedSlotResponseAdaptorMock.Setup(a => a.CreateBookedSlotViewModel(It.IsAny<Result<BookedSlotModel>>())).Returns(bookedSlotResponseAdaptorMockResponse);

            var response = await customerBookedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            bookedSlotResponseAdaptorMock.Verify((m => m.CreateBookedSlotViewModel(It.IsAny<Result<BookedSlotModel>>())), Times.Once());
        }



        [Test]
        public async Task GetCustomerCancelledSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Result<IEnumerable<CancelledSlotInformationModel>> customerBookedSlotBusinessMockResponse = new Result<IEnumerable<CancelledSlotInformationModel>>() { ResultType = ResultType.Empty };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerCancelledSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerCancelledSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Result<IEnumerable<CancelledSlotInformationModel>> customerBookedSlotBusinessMockResponse = new Result<IEnumerable<CancelledSlotInformationModel>>() { Value = new List<CancelledSlotInformationModel>() };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));

            var response = await customerBookedSlotController.GetCustomerCancelledSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
        }

        private BookedSlotModel CreateDefaultBookedSlotModel()
        {
            var bookedSlotModel = new BookedSlotModel();
            bookedSlotModel.CustomerSettingsModel = new CustomerSettingsModel() { TimeZone = IndianTimezone };
            bookedSlotModel.BookedSlotModels = new List<KeyValuePair<CustomerModel, SlotInforamtionInCustomerTimeZoneModel>>();
            bookedSlotModel.BookedSlotModels.Add(new KeyValuePair<CustomerModel, SlotInforamtionInCustomerTimeZoneModel>(CreateDefaultCustomerModel(), CreateDefaultSlotInforamtionInCustomerTimeZoneModel()));

            return bookedSlotModel;
        }

        private BookedSlotViewModel CreateDefaultBookedSlotViewModel()
        {
            return new BookedSlotViewModel();
        }




        private CustomerModel CreateDefaultCustomerModel()
        {
            var customerModel = new CustomerModel();
            customerModel.FirstName = FirstName;
            customerModel.LastName = LastName;
            customerModel.BioHeadLine = BioHeadLine;
            return customerModel;
        }

        private SlotInforamtionInCustomerTimeZoneModel CreateDefaultSlotInforamtionInCustomerTimeZoneModel()
        {
            var slotInforamtionInCustomerTimeZoneModel = new SlotInforamtionInCustomerTimeZoneModel();
            slotInforamtionInCustomerTimeZoneModel.SlotModel = new SlotModel();
            slotInforamtionInCustomerTimeZoneModel.CustomerSlotZonedDateTime = new ZonedDateTime();
            return slotInforamtionInCustomerTimeZoneModel;
        }

    }



}
