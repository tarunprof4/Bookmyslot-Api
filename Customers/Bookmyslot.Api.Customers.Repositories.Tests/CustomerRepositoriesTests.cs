using Bookmyslot.Api.Customers.Contracts;
using Moq;
using NUnit.Framework;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Bookmyslot.Api.Customers.Repositories.Enitites;
using System;
using Bookmyslot.Api.Common;
using Moq.Dapper;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.Common.Contracts;

namespace Bookmyslot.Api.Customers.Repositories.Tests
{
    public class Tests
    {
        private const string EMAIL = "a@gmail.com";
        private const string FIRSTNAME = "fisrtname";
        private const string LASTNAME = "lastname";
        private const string PROVIDER = "provider";

        private RegisterCustomerRepository registerCustomerRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            registerCustomerRepository = new RegisterCustomerRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task RegisterCustomer_ReturnsSuccessResponse()
        {
            var registerCustomerModel = CreateRegisterCustomerModel();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<string>>>())).Returns(Task.FromResult(registerCustomerModel.Email));

            var registerCustomerModelResponse = await registerCustomerRepository.RegisterCustomer(registerCustomerModel);

            Assert.AreEqual(registerCustomerModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(EMAIL, registerCustomerModelResponse.Result);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<string>>>()), Times.Once);
        }

        private RegisterCustomerModel CreateRegisterCustomerModel()
        {
            var registerCustomerModel = new RegisterCustomerModel();
            registerCustomerModel.FirstName = FIRSTNAME;
            registerCustomerModel.LastName = LASTNAME;
            registerCustomerModel.Email = EMAIL;
            registerCustomerModel.Provider = PROVIDER;
            return registerCustomerModel;
        }

        private RegisterCustomerEntity CreateRegisterCustomerEntity()
        {
            var registerCustomerEntity = new RegisterCustomerEntity();
            registerCustomerEntity.FirstName = FIRSTNAME;
            registerCustomerEntity.LastName = LASTNAME;
            registerCustomerEntity.Email = EMAIL;
            registerCustomerEntity.Provider = PROVIDER;
            return registerCustomerEntity;
        }


    }
}