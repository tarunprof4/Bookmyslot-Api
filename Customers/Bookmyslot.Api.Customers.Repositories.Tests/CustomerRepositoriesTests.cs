//using Bookmyslot.Api.Customers.Contracts;
//using Moq;
//using NUnit.Framework;
//using System.Data;
//using System.Threading.Tasks;
//using Dapper;
//using Bookmyslot.Api.Customers.Repositories.Enitites;
//using System;
//using Bookmyslot.Api.Common;
//using Moq.Dapper;

//namespace Bookmyslot.Api.Customers.Repositories.Tests
//{
//    public class Tests
//    {
//        private const string EMAIL = "a@gmail.com";
//        private const string GENDERPREFIX = "genderprefix";
//        private const string FIRSTNAME = "fisrtname";
//        private const string MIDDLENAME = "middlename";
//        private const string LASTNAME = "lastname";
//        private const string GENDER = "gender";
//        private DateTime MODIFIEDDATE = DateTime.Now;

//        private CustomerRepository customerRepository;
//        private Mock<IDbConnection> dbConnectionMock;

//        [SetUp]
//        public void SetUp()
//        {
//            dbConnectionMock = new Mock<IDbConnection>();
//            customerRepository = new CustomerRepository(dbConnectionMock.Object);
//        }

//        [Test]
//        public async Task CustomerBusiness_GetCustomerByEmail_CallsDatabaseAndReturnCustomerResponse()
//        {
//            var customerEntity = CreateCustomerEntity();
//            dbConnectionMock.SetupDapperAsync(m => m.GetAsync<CustomerEntity>(It.IsAny<string>())).Returns(Task.FromResult(customerEntity));
//            dbConnectionMock.Setup(m => m.GetAsync<CustomerEntity>(It.IsAny<string>())).Returns(Task.FromResult(customerEntity));


//            var customerModelResponse = await customerRepository.GetCustomer(EMAIL);
//            var customerModel = customerModelResponse.Result;

//            dbConnectionMock.Verify((m => m.GetAsync<CustomerEntity>(EMAIL, null, null)), Times.Once());
//            Assert.AreEqual(customerModelResponse.ResultType, ResultType.Error);
//            Assert.AreEqual(GENDERPREFIX, customerModel.GenderPrefix);
//            Assert.AreEqual(FIRSTNAME, customerModel.FirstName);
//            Assert.AreEqual(MIDDLENAME, customerModel.MiddleName);
//            Assert.AreEqual(LASTNAME, customerModel.LastName);
//            Assert.AreEqual(GENDER, customerModel.Gender);
//            Assert.AreEqual(EMAIL, customerModel.Email);
//        }

//        private CustomerModel CreateCustomer()
//        {
//            var customerModel = new CustomerModel();
//            customerModel.GenderPrefix = GENDERPREFIX;
//            customerModel.FirstName = FIRSTNAME;
//            customerModel.MiddleName = MIDDLENAME;
//            customerModel.LastName = LASTNAME;
//            customerModel.Gender = GENDER;
//            customerModel.Email = EMAIL;
//            return customerModel;
//        }

//        private CustomerEntity CreateCustomerEntity()
//        {
//            var customerEntity = new CustomerEntity();
//            customerEntity.GenderPrefix = GENDERPREFIX;
//            customerEntity.FirstName = FIRSTNAME;
//            customerEntity.MiddleName = MIDDLENAME;
//            customerEntity.LastName = LASTNAME;
//            customerEntity.Gender = GENDER;
//            customerEntity.Email = EMAIL;
//            customerEntity.ModifiedDate = MODIFIEDDATE;

//            return customerEntity;
//        }
//    }
//}