using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{

    public class RegisterCustomerControllerTests
    {
        private const string CustomerId = "CustomerId";

        private const string ValidFirstName = "ValidFirstName";
        private const string ValidLastName = "ValidLastName";
        private const string ValidEmail = "a@gmail.com";

        private const string InValidFirstName = "InValidFirstName12212";
        private const string InValidLastName = "InValidLastName121212";
        private const string InValidEmail = "InValidEmail";

        private RegisterCustomerController registerCustomerController;
        private Mock<IRegisterCustomerBusiness> registerCustomerBusinessMock;

        [SetUp]
        public void Setup()
        {
            registerCustomerBusinessMock = new Mock<IRegisterCustomerBusiness>();
            registerCustomerController = new RegisterCustomerController(registerCustomerBusinessMock.Object);
        }


        [Test]
        public async Task SaveRegisterCustomer_NullRegisterCustomer_ReturnsValidationResponse()
        {
            var response = await registerCustomerController.Post(null);

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.RegisterCustomerDetailsMissing));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
        }

        [Test]
        public async Task SaveRegisterCustomer_EmptyRegisterCustomer_ReturnsValidationResponse()
        {
            var response = await registerCustomerController.Post(new RegisterCustomerViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.FirstNameRequired));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.LastNameRequired));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.EmailRequired));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
        }





        [Test]
        public async Task SaveRegisterCustomer_InValidRegisterCustomer_ReturnsValidationResponse()
        {
            var response = await registerCustomerController.Post(DefaultInValidRegisterCustomerViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.FirstNameInValid));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.LastNameInValid));
            Assert.IsTrue(validationMessages.Contains(AppBusinessMessagesConstants.EmailIdNotValid));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
        }



        [Test]
        public async Task SaveRegisterCustomer_ValidRegisterCustomer_ReturnsSuccessResponse()
        {
            Response<string> registerCustomerBusinessMockResponse = new Response<string>() { Result = string.Empty };
            registerCustomerBusinessMock.Setup(a => a.RegisterCustomer(It.IsAny<RegisterCustomerModel>())).Returns(Task.FromResult(registerCustomerBusinessMockResponse));


            var response = await registerCustomerController.Post(DefaultValidRegisterCustomerViewModel());

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Once());


        }


        private RegisterCustomerViewModel DefaultValidRegisterCustomerViewModel()
        {
            return new RegisterCustomerViewModel()
            {
                FirstName = ValidFirstName,
                LastName = ValidLastName,
                Email = ValidEmail
            };
        }

        private RegisterCustomerViewModel DefaultInValidRegisterCustomerViewModel()
        {
            return new RegisterCustomerViewModel()
            {
                FirstName = InValidFirstName,
                LastName = InValidLastName,
                Email = InValidEmail
            };
        }

    }
}
