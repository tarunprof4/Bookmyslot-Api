using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Event.Interfaces;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Customers.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories.Tests
{
    public class RegisterCustomerRepositoriesTests
    {
        private const string EMAIL = "a@gmail.com";
        private const string FIRSTNAME = "fisrtname";
        private const string LASTNAME = "lastname";
        private const string PROVIDER = "provider";

        private RegisterCustomerRepository registerCustomerRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;
        private Mock<IEventDispatcher> eventDispatcherMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            eventDispatcherMock = new Mock<IEventDispatcher>();
            registerCustomerRepository = new RegisterCustomerRepository(dbConnectionMock.Object, dbInterceptorMock.Object,
                eventDispatcherMock.Object);
        }

        [Test]
        public async Task RegisterCustomer_ReturnsSuccessResponse()
        {
            var registerCustomerModel = DefaultCreateRegisterCustomerModel();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var registerCustomerModelResponse = await registerCustomerRepository.RegisterCustomer(registerCustomerModel);

            Assert.AreEqual(registerCustomerModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(EMAIL, registerCustomerModelResponse.Result);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }

        private RegisterCustomerModel DefaultCreateRegisterCustomerModel()
        {
            var registerCustomerModel = new RegisterCustomerModel();
            registerCustomerModel.FirstName = FIRSTNAME;
            registerCustomerModel.LastName = LASTNAME;
            registerCustomerModel.Email = EMAIL;
            registerCustomerModel.Provider = PROVIDER;
            return registerCustomerModel;
        }




    }
}