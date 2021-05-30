using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.AdaptorTests
{

    [TestFixture]
    public class SharedSlotResponseAdaptorTests
    {
        private const string NoRecordsFound = "NoRecordsFound";
        private const string SlotInformation = "SlotInformation";
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<ICustomerResponseAdaptor> customerResponseAdaptorMock;
        private SharedSlotResponseAdaptor sharedSlotResponseAdaptor;

        [SetUp]
        public void Setup()
        {
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            customerResponseAdaptorMock = new Mock<ICustomerResponseAdaptor>();
            sharedSlotResponseAdaptor = new SharedSlotResponseAdaptor(symmetryEncryptionMock.Object, customerResponseAdaptorMock.Object);
        }


        [Test]
        public void CreateSharedSlotViewModel_EmptySharedSlotModel_ReturnsEmptyCreateSharedSlotViewModel()
        {
            var bookAvailableSlotViewModelResponse = sharedSlotResponseAdaptor.CreateSharedSlotViewModel(CreateEmptyDefaultSharedSlotModel());

            Assert.AreEqual(bookAvailableSlotViewModelResponse.ResultType, ResultType.Empty);
            Assert.AreEqual(bookAvailableSlotViewModelResponse.Messages[0], NoRecordsFound);
        }

        [Test]
        public void CreateSharedSlotViewModel_ValidSharedSlotModelNotBookedYet_ReturnsValidCreateSharedSlotViewModel()
        {
            CustomerViewModel customerViewModel = null;
            customerResponseAdaptorMock.Setup(a => a.CreateCustomerViewModel(It.IsAny<CustomerModel>())).Returns(customerViewModel);
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(SlotInformation);
            var sharedSlotModel = CreateDefaultSharedSlotModel();
            sharedSlotModel.Result.SharedSlotModels[0] = new KeyValuePair<CustomerModel, SlotModel>(null, new SlotModel());

            var bookedSlotViewModelResponse = sharedSlotResponseAdaptor.CreateSharedSlotViewModel(sharedSlotModel);

            var bookedSlotViewModel = bookedSlotViewModelResponse.Result;
            Assert.AreEqual(bookedSlotViewModelResponse.ResultType, ResultType.Success);
            Assert.IsNull(bookedSlotViewModel.SharedSlotModels[0].Item1);
            Assert.AreEqual(bookedSlotViewModel.SharedSlotModels[0].Item3, SlotInformation);
        }

        [Test]
        public void CreateSharedSlotViewModel_ValidSharedSlotModelBookedSlot_ReturnsValidCreateSharedSlotViewModel()
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();
            customerResponseAdaptorMock.Setup(a => a.CreateCustomerViewModel(It.IsAny<CustomerModel>())).Returns(customerViewModel);
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(SlotInformation);
            var sharedSlotModel = CreateDefaultSharedSlotModel();
            
            var bookedSlotViewModelResponse = sharedSlotResponseAdaptor.CreateSharedSlotViewModel(sharedSlotModel);

            var bookedSlotViewModel = bookedSlotViewModelResponse.Result;
            Assert.AreEqual(bookedSlotViewModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(bookedSlotViewModel.SharedSlotModels[0].Item1);
            Assert.AreEqual(bookedSlotViewModel.SharedSlotModels[0].Item3, SlotInformation);
        }

        private Response<SharedSlotModel> CreateEmptyDefaultSharedSlotModel()
        {
            var emptySharedSlotModelResponse = new Response<SharedSlotModel>()
            {
                ResultType = ResultType.Empty,
                Messages = new List<string>() { NoRecordsFound }
            };

            return emptySharedSlotModelResponse;
        }

        private Response<SharedSlotModel> CreateDefaultSharedSlotModel()
        {
            var sharedSlotModel = new SharedSlotModel();
            sharedSlotModel.SharedSlotModels = new List<KeyValuePair<CustomerModel, SlotModel>>();
            sharedSlotModel.SharedSlotModels.Add(new KeyValuePair<CustomerModel, SlotModel>(new CustomerModel(), new SlotModel()));

            var sharedSlotModelResponse = new Response<SharedSlotModel>() { Result = sharedSlotModel };
            return sharedSlotModelResponse;
        }

     

    }
}
