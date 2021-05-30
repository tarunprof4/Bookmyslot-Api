using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.AdaptorTests
{


    [TestFixture]
    public class CustomerResponseAdaptorTests
    {
        private const string Id = "Id";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string BioHeadLine = "BioHeadLine";
        private const string ProfilePictureUrl = "ProfilePictureUrl";
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private CustomerResponseAdaptor customerResponseAdaptor;

        [SetUp]
        public void Setup()
        {
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            customerResponseAdaptor = new CustomerResponseAdaptor(symmetryEncryptionMock.Object);
        }


        [Test]
        public void CreateCustomerViewModel_ValidCustomerModel_ReturnsSuccessCustomerViewModel()
        {
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(Id);

            var customerViewModel = customerResponseAdaptor.CreateCustomerViewModel(CreateCustomerModel());

            Assert.AreEqual(customerViewModel.Id, Id);
            Assert.AreEqual(customerViewModel.FirstName, FirstName);
            Assert.AreEqual(customerViewModel.LastName, LastName);
            Assert.AreEqual(customerViewModel.BioHeadLine, BioHeadLine);
            Assert.AreEqual(customerViewModel.ProfilePictureUrl, ProfilePictureUrl);
        }


        [Test]
        public void CreateCustomerViewModels_ValidCustomerModels_ReturnsSuccessCustomerViewModels()
        {
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(Id);

            var customerViewModels = customerResponseAdaptor.CreateCustomerViewModels(new List<CustomerModel>() { CreateCustomerModel() });
            var customerViewModelsList = customerViewModels.ToList();

            Assert.AreEqual(customerViewModelsList[0].Id, Id);
            Assert.AreEqual(customerViewModelsList[0].FirstName, FirstName);
            Assert.AreEqual(customerViewModelsList[0].LastName, LastName);
            Assert.AreEqual(customerViewModelsList[0].BioHeadLine, BioHeadLine);
            Assert.AreEqual(customerViewModelsList[0].ProfilePictureUrl, ProfilePictureUrl);
        }


        [Test]
        public void CreateCustomerViewModels_ValidCustomerSlotModels_ReturnsSuccessCustomerViewModels()
        {
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(Id);

            var customerViewModels = customerResponseAdaptor.CreateCustomerViewModels(new List<CustomerSlotModel>() { CreateCustomerSlotModel() });
            var customerViewModelsList = customerViewModels.ToList();

            Assert.AreEqual(customerViewModelsList[0].Id, Id);
            Assert.AreEqual(customerViewModelsList[0].FirstName, FirstName);
            Assert.AreEqual(customerViewModelsList[0].LastName, LastName);
            Assert.AreEqual(customerViewModelsList[0].BioHeadLine, BioHeadLine);
            Assert.AreEqual(customerViewModelsList[0].ProfilePictureUrl, ProfilePictureUrl);
        }


        private CustomerModel CreateCustomerModel()
        {
            return new CustomerModel()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                BioHeadLine = BioHeadLine,
                ProfilePictureUrl = ProfilePictureUrl
            };
        }

        private CustomerSlotModel CreateCustomerSlotModel()
        {
            return new CustomerSlotModel() { CustomerModel = CreateCustomerModel() };
        }





    }
}
