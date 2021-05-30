using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Tests
{
    public class CustomerLastSharedSlotRepositoryTests
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

        private CustomerLastSharedSlotRepository customerLastSharedSlotRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            customerLastSharedSlotRepository = new CustomerLastSharedSlotRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task GetCustomerLatestSlot_NoRecordFound_ReturnsEmptyResponse()
        {
            CustomerLastSharedSlotEntity customerLastBookedSlotEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerLastSharedSlotEntity>>>())).Returns(Task.FromResult(customerLastBookedSlotEntity));

            var customerLastBookedSlotResponse = await customerLastSharedSlotRepository.GetCustomerLatestSharedSlot(CustomerId);

            Assert.AreEqual(customerLastBookedSlotResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerLastSharedSlotEntity>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerLatestSlot_HasRecord_ReturnsSuccessResponse()
        {
            CustomerLastSharedSlotEntity customerLastBookedSlotEntity = DefaultCreateCustomerLastBookedSlotEntity();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerLastSharedSlotEntity>>>())).Returns(Task.FromResult(customerLastBookedSlotEntity));

            var customerLastBookedSlotResponse = await customerLastSharedSlotRepository.GetCustomerLatestSharedSlot(CustomerId);
            var customerLastBookedSlot = customerLastBookedSlotResponse.Result;

            Assert.AreEqual(customerLastBookedSlotResponse.ResultType, ResultType.Success);
            Assert.AreEqual(customerLastBookedSlot.CreatedBy, CustomerId);
            Assert.AreEqual(customerLastBookedSlot.Title, Title);
            Assert.AreEqual(customerLastBookedSlot.Country, Country);
            Assert.AreEqual(customerLastBookedSlot.SlotStartZonedDateTime.Zone.Id, TimeZone);
            Assert.AreEqual(customerLastBookedSlot.SlotStartTime, SlotStartTime);
            Assert.AreEqual(customerLastBookedSlot.SlotEndTime, SlotEndTime);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<CustomerLastSharedSlotEntity>>>()), Times.Once);
        }




        [Test]
        public async Task SaveCustomerLatestSlot_ValidSaveCustomerLatestSlotModel_ReturnsSuccessResponse()
        {
            CustomerLastSharedSlotModel customerLastBookedSlotModel = DefaultCreateCustomerLastBookedSlotModel();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var customerLastBookedSlotResponse = await customerLastSharedSlotRepository.SaveCustomerLatestSharedSlot(customerLastBookedSlotModel);

            Assert.AreEqual(customerLastBookedSlotResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }



        private CustomerLastSharedSlotEntity DefaultCreateCustomerLastBookedSlotEntity()
        {
            var customerLastBookedSlotEntity = new CustomerLastSharedSlotEntity();

            customerLastBookedSlotEntity.CreatedBy = CustomerId;
            customerLastBookedSlotEntity.Title = Title;
            customerLastBookedSlotEntity.Country = Country;
            customerLastBookedSlotEntity.TimeZone = TimeZone;
            customerLastBookedSlotEntity.SlotDate = SlotDate;
            customerLastBookedSlotEntity.SlotStartDateTimeUtc = SlotDateUtc;
            customerLastBookedSlotEntity.SlotStartTime = SlotStartTime;
            customerLastBookedSlotEntity.SlotEndTime = SlotEndTime;
            customerLastBookedSlotEntity.ModifiedDateUtc = ModifiedDateUtc;

            return customerLastBookedSlotEntity;
        }

        private CustomerLastSharedSlotModel DefaultCreateCustomerLastBookedSlotModel()
        {
            var customerLastBookedSlotModel = new CustomerLastSharedSlotModel();
            return customerLastBookedSlotModel;
        }



    }
}
