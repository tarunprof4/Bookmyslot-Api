using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.ValueObject;
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
            sharedSlotModel.Value.SharedSlotModels[0] = new KeyValuePair<CustomerModel, SlotModel>(null, new SlotModel());

            var bookedSlotViewModelResponse = sharedSlotResponseAdaptor.CreateSharedSlotViewModel(sharedSlotModel);

            var bookedSlotViewModel = bookedSlotViewModelResponse.Value;
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

            var bookedSlotViewModel = bookedSlotViewModelResponse.Value;
            Assert.AreEqual(bookedSlotViewModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(bookedSlotViewModel.SharedSlotModels[0].Item1);
            Assert.AreEqual(bookedSlotViewModel.SharedSlotModels[0].Item3, SlotInformation);
        }

        private Result<SharedSlotModel> CreateEmptyDefaultSharedSlotModel()
        {
            var emptySharedSlotModelResponse = new Result<SharedSlotModel>()
            {
                ResultType = ResultType.Empty,
                Messages = new List<string>() { NoRecordsFound }
            };

            return emptySharedSlotModelResponse;
        }

        private Result<SharedSlotModel> CreateDefaultSharedSlotModel()
        {
            var sharedSlotModel = new SharedSlotModel();
            sharedSlotModel.SharedSlotModels = new List<KeyValuePair<CustomerModel, SlotModel>>();
            sharedSlotModel.SharedSlotModels.Add(new KeyValuePair<CustomerModel, SlotModel>(new CustomerModel(), new SlotModel()));

            var sharedSlotModelResponse = new Result<SharedSlotModel>() { Value = sharedSlotModel };
            return sharedSlotModelResponse;
        }



    }
}
