using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.SharedKernel.ValueObject;
using FluentValidation;
using FluentValidation.Results;
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

        private const string ValidFirstName = "ValidFirstName";
        private const string ValidLastName = "ValidLastName";
        private const string ValidEmail = "a@gmail.com";

        private const string InValidFirstName = "InValidFirstName12212";
        private const string InValidLastName = "InValidLastName121212";
        private const string InValidEmail = "InValidEmail";

        private const string InValiRegisterCustomer = "InValiRegisterCustomer";

        private RegisterCustomerController registerCustomerController;
        private Mock<IRegisterCustomerBusiness> registerCustomerBusinessMock;
        private Mock<IValidator<RegisterCustomerViewModel>> registerCustomerViewModelValidatorMock;

        [SetUp]
        public void Setup()
        {
            registerCustomerBusinessMock = new Mock<IRegisterCustomerBusiness>();
            registerCustomerViewModelValidatorMock = new Mock<IValidator<RegisterCustomerViewModel>>();
            registerCustomerController = new RegisterCustomerController(registerCustomerBusinessMock.Object,
                registerCustomerViewModelValidatorMock.Object);
        }



        [Test]
        public async Task SaveRegisterCustomer_InValidRegisterCustomer_ReturnsValidationResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailure();
            registerCustomerViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<RegisterCustomerViewModel>())).Returns(new ValidationResult(validationFailures));

            var response = await registerCustomerController.Post(DefaultInValidRegisterCustomerViewModel());

            var objectResult = response as ObjectResult;
            var validationMessages = objectResult.Value as List<string>;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.IsTrue(validationMessages.Contains(InValiRegisterCustomer));
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Never());
            registerCustomerViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<RegisterCustomerViewModel>())), Times.Once());
        }



        [Test]
        public async Task SaveRegisterCustomer_ValidRegisterCustomer_ReturnsSuccessResponse()
        {
            registerCustomerViewModelValidatorMock.Setup(a => a.Validate(It.IsAny<RegisterCustomerViewModel>())).Returns(new ValidationResult());
            Result<string> registerCustomerBusinessMockResponse = new Result<string>() { Value = string.Empty };
            registerCustomerBusinessMock.Setup(a => a.RegisterCustomer(It.IsAny<RegisterCustomerModel>())).Returns(Task.FromResult(registerCustomerBusinessMockResponse));

            var response = await registerCustomerController.Post(DefaultValidRegisterCustomerViewModel());

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status201Created);
            registerCustomerBusinessMock.Verify((m => m.RegisterCustomer(It.IsAny<RegisterCustomerModel>())), Times.Once());
            registerCustomerViewModelValidatorMock.Verify((m => m.Validate(It.IsAny<RegisterCustomerViewModel>())), Times.Once());
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

        private static List<ValidationFailure> CreateDefaultValidationFailure()
        {
            ValidationFailure validationFailure = new ValidationFailure("", InValiRegisterCustomer);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            return validationFailures;
        }
    }
}
