//using Bookmyslot.Api.Customers.Contracts;
//using Moq;
//using NUnit.Framework;
//using System.Data;
//using System.Threading.Tasks;
//using Dapper;
//using Bookmyslot.Api.Customers.Repositories.Enitites;
//using System;

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
//            dbConnectionMock.Setup(c => c.GetAsync<CustomerEntity>(EMAIL))
//              .Returns(expected);
//            dbConnectionMock.Setup(a => a.GetAsync<CustomerEntity>(EMAIL)).Returns(Task.FromResult(customerEntity));

//            var customer = await customerRepository.GetCustomer(EMAIL);

//            dbConnectionMock.Verify((m => m.GetAsync<CustomerEntity>("")
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