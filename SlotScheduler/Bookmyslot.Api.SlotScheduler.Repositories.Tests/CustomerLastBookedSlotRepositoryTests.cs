using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Tests
{
    public class CustomerLastBookedSlotRepositoryTests
    {
        private const string CustomerId = "CustomerId";
        private const string Country = "Country";

        private readonly string Id = Guid.NewGuid().ToString();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private const string BookedBy = "BookedBy";
        private const string TimeZone = TimeZoneConstants.IndianTimezone;
        private const string SlotDate = "03-22-2021";
        private readonly DateTime SlotDateUtc = DateTime.UtcNow;
        private readonly TimeSpan SlotStartTime = new TimeSpan(1, 1, 1);
        private readonly TimeSpan SlotEndTime = new TimeSpan(2, 2, 2);
        private readonly DateTime CreatedDateUtc = DateTime.UtcNow;
        private readonly DateTime ModifiedDateUtc = DateTime.UtcNow;

        private CustomerLastBookedSlotRepository customerLastBookedSlotRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            customerLastBookedSlotRepository = new CustomerLastBookedSlotRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task GetCustomerLatestSlot_NoRecordFound_ReturnsEmptyResponse()
        {
            CustomerLastBookedSlotEntity customerLastBookedSlotEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerLastBookedSlotEntity>>>())).Returns(Task.FromResult(customerLastBookedSlotEntity));

            var customerLastBookedSlotResponse = await customerLastBookedSlotRepository.GetCustomerLatestSlot(CustomerId);

            Assert.AreEqual(customerLastBookedSlotResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerLastBookedSlotEntity>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerLatestSlot_HasRecord_ReturnsSuccessResponse()
        {
            CustomerLastBookedSlotEntity customerLastBookedSlotEntity = DefaultCreateCustomerLastBookedSlotEntity();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerLastBookedSlotEntity>>>())).Returns(Task.FromResult(customerLastBookedSlotEntity));

            var customerLastBookedSlotResponse = await customerLastBookedSlotRepository.GetCustomerLatestSlot(CustomerId);
            var customerLastBookedSlot = customerLastBookedSlotResponse.Result;

            Assert.AreEqual(customerLastBookedSlotResponse.ResultType, ResultType.Success);
            Assert.AreEqual(customerLastBookedSlot.CreatedBy, CustomerId);
            Assert.AreEqual(customerLastBookedSlot.Title, Title);
            Assert.AreEqual(customerLastBookedSlot.Country, Country);
            Assert.AreEqual(customerLastBookedSlot.SlotZonedDate.Zone.Id, TimeZone);
            Assert.AreEqual(customerLastBookedSlot.SlotStartTime, SlotStartTime);
            Assert.AreEqual(customerLastBookedSlot.SlotEndTime, SlotEndTime);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerLastBookedSlotEntity>>>()), Times.Once);
        }




        [Test]
        public async Task SaveCustomerLatestSlot_ValidSaveCustomerLatestSlotModel_ReturnsSuccessResponse()
        {
            CustomerLastBookedSlotModel customerLastBookedSlotModel = DefaultCreateCustomerLastBookedSlotModel();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var customerLastBookedSlotResponse = await customerLastBookedSlotRepository.SaveCustomerLatestSlot(customerLastBookedSlotModel);

            Assert.AreEqual(customerLastBookedSlotResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }



        private CustomerLastBookedSlotEntity DefaultCreateCustomerLastBookedSlotEntity()
        {
            var customerLastBookedSlotEntity = new CustomerLastBookedSlotEntity();

            customerLastBookedSlotEntity.CreatedBy = CustomerId;
            customerLastBookedSlotEntity.Title = Title;
            customerLastBookedSlotEntity.Country = Country;
            customerLastBookedSlotEntity.TimeZone = TimeZone;
            customerLastBookedSlotEntity.SlotDate = SlotDate;
            customerLastBookedSlotEntity.SlotDateUtc = SlotDateUtc;
            customerLastBookedSlotEntity.SlotStartTime = SlotStartTime;
            customerLastBookedSlotEntity.SlotEndTime = SlotEndTime;
            customerLastBookedSlotEntity.ModifiedDateUtc = ModifiedDateUtc;

            return customerLastBookedSlotEntity;
        }

        private CustomerLastBookedSlotModel DefaultCreateCustomerLastBookedSlotModel()
        {
            var customerLastBookedSlotModel = new CustomerLastBookedSlotModel();
            return customerLastBookedSlotModel;
        }



    }
}
