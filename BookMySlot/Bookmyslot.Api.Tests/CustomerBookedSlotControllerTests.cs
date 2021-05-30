using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
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

            Response<CurrentUserModel> currentUserMockResponse = new Response<CurrentUserModel>() { Result = new CurrentUserModel() { Id= CustomerId, FirstName = FirstName } };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetCustomerBookedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<BookedSlotModel> customerBookedSlotBusinessMockResponse = new Response<BookedSlotModel>() { ResultType = ResultType.Empty };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));
            Response<BookedSlotViewModel> bookedSlotResponseAdaptorMockResponse = new Response<BookedSlotViewModel>() { ResultType = ResultType.Empty };
            bookedSlotResponseAdaptorMock.Setup(a => a.CreateBookedSlotViewModel(It.IsAny<Response<BookedSlotModel>>())).Returns(bookedSlotResponseAdaptorMockResponse);

            var response = await customerBookedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            bookedSlotResponseAdaptorMock.Verify((m => m.CreateBookedSlotViewModel(It.IsAny<Response<BookedSlotModel>>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerBookedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<BookedSlotModel> customerBookedSlotBusinessMockResponse = new Response<BookedSlotModel>() { Result = CreateDefaultBookedSlotModel() };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));
            Response<BookedSlotViewModel> bookedSlotResponseAdaptorMockResponse = new Response<BookedSlotViewModel>() { Result = CreateDefaultBookedSlotViewModel() };
            bookedSlotResponseAdaptorMock.Setup(a => a.CreateBookedSlotViewModel(It.IsAny<Response<BookedSlotModel>>())).Returns(bookedSlotResponseAdaptorMockResponse);

            var response = await customerBookedSlotController.GetCustomerBookedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            bookedSlotResponseAdaptorMock.Verify((m => m.CreateBookedSlotViewModel(It.IsAny<Response<BookedSlotModel>>())), Times.Once());
        }



        [Test]
        public async Task GetCustomerCompletedSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<BookedSlotModel> customerBookedSlotBusinessMockResponse = new Response<BookedSlotModel>() { ResultType = ResultType.Empty };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));
            Response<BookedSlotViewModel> bookedSlotResponseAdaptorMockResponse = new Response<BookedSlotViewModel>() { ResultType = ResultType.Empty };
            bookedSlotResponseAdaptorMock.Setup(a => a.CreateBookedSlotViewModel(It.IsAny<Response<BookedSlotModel>>())).Returns(bookedSlotResponseAdaptorMockResponse);


            var response = await customerBookedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status404NotFound);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            bookedSlotResponseAdaptorMock.Verify((m => m.CreateBookedSlotViewModel(It.IsAny<Response<BookedSlotModel>>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerCompletedSlots_RecordsAvailable_ReturnsSuccessResponse()
        {
            Response<BookedSlotModel> customerBookedSlotBusinessMockResponse = new Response<BookedSlotModel>() { Result = CreateDefaultBookedSlotModel() };
            customerBookedSlotBusinessMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(customerBookedSlotBusinessMockResponse));
            Response<BookedSlotViewModel> bookedSlotResponseAdaptorMockResponse = new Response<BookedSlotViewModel>() { Result = CreateDefaultBookedSlotViewModel() };
            bookedSlotResponseAdaptorMock.Setup(a => a.CreateBookedSlotViewModel(It.IsAny<Response<BookedSlotModel>>())).Returns(bookedSlotResponseAdaptorMockResponse);

            var response = await customerBookedSlotController.GetCustomerCompletedSlots();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
            customerBookedSlotBusinessMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            bookedSlotResponseAdaptorMock.Verify((m => m.CreateBookedSlotViewModel(It.IsAny<Response<BookedSlotModel>>())), Times.Once());
        }



        [Test]
        public async Task GetCustomerCancelledSlots_NoRecordsAvailable_ReturnsEmptyResponse()
        {
            Response<IEnumerable<CancelledSlotInformationModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<CancelledSlotInformationModel>>() { ResultType = ResultType.Empty };
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
            Response<IEnumerable<CancelledSlotInformationModel>> customerBookedSlotBusinessMockResponse = new Response<IEnumerable<CancelledSlotInformationModel>>() { Result = new List<CancelledSlotInformationModel>() };
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
