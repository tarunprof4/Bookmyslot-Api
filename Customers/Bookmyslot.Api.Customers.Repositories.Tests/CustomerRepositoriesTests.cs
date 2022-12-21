using Bookmyslot.Api.Customers.Repositories.Enitites;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Database;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Repositories.Tests
{
    public class CustomerRepositoriesTests
    {
        private const string Id = "id";
        private const string EMAIL = "a@gmail.com";
        private const string FIRSTNAME = "fisrtname";
        private const string LASTNAME = "lastname";
        private const string BioHeadLine = "BioHeadLine";
        private const string CustomerId = "CustomerId";

        private CustomerRepository customerRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            customerRepository = new CustomerRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task GetCustomerSettings_NoRecordsFound_ReturnsEmptyResponse()
        {
            RegisterCustomerEntity registerCustomerEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(registerCustomerEntity));

            var customerSettingsModelResponse = await customerRepository.GetCustomerByEmail(EMAIL);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerSettings_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(DefaultCreateRegisterCustomerEntity()));

            var customerSettingsModelResponse = await customerRepository.GetCustomerByEmail(EMAIL);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(customerSettingsModelResponse.Value.Id, Id);
            Assert.AreEqual(customerSettingsModelResponse.Value.FirstName, FIRSTNAME);
            Assert.AreEqual(customerSettingsModelResponse.Value.LastName, LASTNAME);
            Assert.AreEqual(customerSettingsModelResponse.Value.BioHeadLine, BioHeadLine);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }



        [Test]
        public async Task GetCustomerIdByEmail_NoRecordsFound_ReturnsEmptyResponse()
        {
            RegisterCustomerEntity registerCustomerEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(registerCustomerEntity));

            var customerSettingsModelResponse = await customerRepository.GetCustomerIdByEmail(EMAIL);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerIdByEmail_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(DefaultCreateRegisterCustomerEntity()));

            var customerSettingsModelResponse = await customerRepository.GetCustomerIdByEmail(EMAIL);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(customerSettingsModelResponse.Value, Id);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }



        [Test]
        public async Task GetCustomerById_NoRecordsFound_ReturnsEmptyResponse()
        {
            RegisterCustomerEntity registerCustomerEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(registerCustomerEntity));

            var customerSettingsModelResponse = await customerRepository.GetCustomerById(CustomerId);

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerById_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>())).Returns(Task.FromResult(DefaultCreateRegisterCustomerEntity()));

            var customerSettingsModelResponse = await customerRepository.GetCustomerById(CustomerId); ;

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(customerSettingsModelResponse.Value.Id, Id);
            Assert.AreEqual(customerSettingsModelResponse.Value.FirstName, FIRSTNAME);
            Assert.AreEqual(customerSettingsModelResponse.Value.LastName, LASTNAME);
            Assert.AreEqual(customerSettingsModelResponse.Value.BioHeadLine, BioHeadLine);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<RegisterCustomerEntity>>>()), Times.Once);
        }


        [Test]
        public async Task GetCustomersByCustomerIds_NoRecordsFound_ReturnsEmptyResponse()
        {
            IEnumerable<RegisterCustomerEntity> registerCustomerEntities = new List<RegisterCustomerEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<RegisterCustomerEntity>>>>())).Returns(Task.FromResult(registerCustomerEntities));

            var customerSettingsModelResponse = await customerRepository.GetCustomersByCustomerIds(new List<string>() { CustomerId, CustomerId });

            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<RegisterCustomerEntity>>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomersByCustomerIds_HasRecord_ReturnsSuccessResponse()
        {
            IEnumerable<RegisterCustomerEntity> registerCustomerEntities = new List<RegisterCustomerEntity>() { DefaultCreateRegisterCustomerEntity(), DefaultCreateRegisterCustomerEntity() };
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<RegisterCustomerEntity>>>>())).Returns(Task.FromResult(registerCustomerEntities));

            var customerSettingsModelResponse = await customerRepository.GetCustomersByCustomerIds(new List<string>() { CustomerId, CustomerId });


            foreach (var customerModel in customerSettingsModelResponse.Value)
            {
                Assert.AreEqual(customerModel.Id, Id);
                Assert.AreEqual(customerModel.FirstName, FIRSTNAME);
                Assert.AreEqual(customerModel.LastName, LASTNAME);
                Assert.AreEqual(customerModel.BioHeadLine, BioHeadLine);
            }
            Assert.AreEqual(customerSettingsModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<RegisterCustomerEntity>>>>()), Times.Once);
        }


        private RegisterCustomerEntity DefaultCreateRegisterCustomerEntity()
        {
            var registerCustomerEntity = new RegisterCustomerEntity();
            registerCustomerEntity.Id = Id;
            registerCustomerEntity.FirstName = FIRSTNAME;
            registerCustomerEntity.LastName = LASTNAME;
            registerCustomerEntity.Email = EMAIL;
            registerCustomerEntity.BioHeadLine = BioHeadLine;
            return registerCustomerEntity;
        }


    }
}