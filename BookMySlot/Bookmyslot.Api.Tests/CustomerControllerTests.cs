using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Authentication.ViewModels;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Tests
{
    public class CustomerControllerTests
    {
        private const string CustomerId = "CustomerId";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string BioHeadLine = "BioHeadLine";
        private const string ProfilePictureUrl = "ProfilePictureUrl";
        private const string UserName = "UserName";
        private const bool IsVerified = true;
        private CustomerController customerController;
        private Mock<ICustomerBusiness> customerBusinessMock;
        private Mock<ICurrentUser> currentUserMock;

        [SetUp]
        public void Setup()
        {
            customerBusinessMock = new Mock<ICustomerBusiness>();
            currentUserMock = new Mock<ICurrentUser>();
            customerController = new CustomerController(customerBusinessMock.Object, currentUserMock.Object);

            Result<CurrentUserModel> currentUserMockResponse = new Result<CurrentUserModel>()
            {
                Value = new CurrentUserModel()
                {
                    Id = CustomerId,
                    FirstName = FirstName,
                    LastName = LastName,
                    BioHeadLine = BioHeadLine,
                    ProfilePictureUrl = ProfilePictureUrl,
                    UserName = UserName,
                    IsVerified = IsVerified
                }
            };
            currentUserMock.Setup(a => a.GetCurrentUserFromCache()).Returns(Task.FromResult(currentUserMockResponse));
        }

        [Test]
        public async Task GetProfileSettings_ReturnsSuccessResponse()
        {
            var response = await customerController.Get();

            var objectResult = response as ObjectResult;
            Assert.AreEqual(objectResult.StatusCode, StatusCodes.Status200OK);
            var currentUserViewModel = objectResult.Value as CurrentUserViewModel;
            Assert.AreEqual(currentUserViewModel.FirstName, FirstName);
            Assert.AreEqual(currentUserViewModel.LastName, LastName);
            Assert.AreEqual(currentUserViewModel.BioHeadLine, BioHeadLine);
            Assert.AreEqual(currentUserViewModel.ProfilePictureUrl, ProfilePictureUrl);
            Assert.AreEqual(currentUserViewModel.UserName, UserName);
            Assert.AreEqual(currentUserViewModel.IsVerified, IsVerified);
            currentUserMock.Verify((m => m.GetCurrentUserFromCache()), Times.Once());
        }
    }
}
