using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Tests
{
    public class CustomerCancelledSlotRepositoryTests
    {
        private const string CustomerId = "CustomerId";

        private readonly string Id = Guid.NewGuid().ToString();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private const string CancelledBy = "CancelledBy";
        private const string BookedBy = "BookedBy";
        private const string TimeZone = TimeZoneConstants.IndianTimezone;
        private const string SlotDate = "Mar 22,2021";
        private readonly DateTime SlotDateUtc = DateTime.UtcNow;
        private readonly TimeSpan SlotStartTime = new TimeSpan(1, 1, 1);
        private readonly TimeSpan SlotEndTime = new TimeSpan(2, 2, 2);
        private readonly DateTime CreatedDateUtc = DateTime.UtcNow;

        private CustomerCancelledSlotRepository customerCancelledSlotRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            customerCancelledSlotRepository = new CustomerCancelledSlotRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task CreateCustomerCancelledSlot_NoRecordsFound_ReturnsEmptyResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<string>>>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));

            var cancelledSlotModelsResponse = await customerCancelledSlotRepository.CreateCustomerCancelledSlot(new CancelledSlotModel());

            Assert.AreEqual(cancelledSlotModelsResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<string>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerSharedCancelledSlots_NoRecordsFound_ReturnsEmptyResponse()
        {
            IEnumerable<CancelledSlotEntity> slotEntities = new List<CancelledSlotEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<CancelledSlotEntity>>>>())).Returns(Task.FromResult(slotEntities));

            var cancelledSlotModelsResponse = await customerCancelledSlotRepository.GetCustomerSharedCancelledSlots(CustomerId);

            Assert.AreEqual(cancelledSlotModelsResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<CancelledSlotEntity>>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerSharedCancelledSlots_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<CancelledSlotEntity>>>>())).Returns(Task.FromResult(DefaultCreateCancelledSlotEntities()));

            var cancelledSlotModelsResponse = await customerCancelledSlotRepository.GetCustomerSharedCancelledSlots(CustomerId);

            foreach (var cancelledSlotModel in cancelledSlotModelsResponse.Result)
            {
                Assert.AreEqual(cancelledSlotModel.Id, Id);
                Assert.AreEqual(cancelledSlotModel.Title, Title);
                Assert.AreEqual(cancelledSlotModel.CreatedBy, CreatedBy);
                Assert.AreEqual(cancelledSlotModel.CancelledBy, CancelledBy);
                Assert.AreEqual(cancelledSlotModel.BookedBy, BookedBy);
                Assert.AreEqual(cancelledSlotModel.SlotZonedDate.Zone.Id, TimeZone);
                Assert.AreEqual(cancelledSlotModel.SlotStartTime, SlotStartTime);
                Assert.AreEqual(cancelledSlotModel.SlotEndTime, SlotEndTime);
            }
            Assert.AreEqual(cancelledSlotModelsResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<CancelledSlotEntity>>>>()), Times.Once);
        }


        [Test]
        public async Task GetCustomerBookedCancelledSlots_NoRecordsFound_ReturnsEmptyResponse()
        {
            IEnumerable<CancelledSlotEntity> slotEntities = new List<CancelledSlotEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<CancelledSlotEntity>>>>())).Returns(Task.FromResult(slotEntities));

            var cancelledSlotModelsResponse = await customerCancelledSlotRepository.GetCustomerBookedCancelledSlots(CustomerId);

            Assert.AreEqual(cancelledSlotModelsResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<CancelledSlotEntity>>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerBookedCancelledSlots_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<CancelledSlotEntity>>>>())).Returns(Task.FromResult(DefaultCreateCancelledSlotEntities()));

            var cancelledSlotModelsResponse = await customerCancelledSlotRepository.GetCustomerBookedCancelledSlots(CustomerId);

            foreach (var cancelledSlotModel in cancelledSlotModelsResponse.Result)
            {
                Assert.AreEqual(cancelledSlotModel.Id, Id);
                Assert.AreEqual(cancelledSlotModel.Title, Title);
                Assert.AreEqual(cancelledSlotModel.CreatedBy, CreatedBy);
                Assert.AreEqual(cancelledSlotModel.CancelledBy, CancelledBy);
                Assert.AreEqual(cancelledSlotModel.BookedBy, BookedBy);
                Assert.AreEqual(cancelledSlotModel.SlotZonedDate.Zone.Id, TimeZone);
                Assert.AreEqual(cancelledSlotModel.SlotStartTime, SlotStartTime);
                Assert.AreEqual(cancelledSlotModel.SlotEndTime, SlotEndTime);
            }
            Assert.AreEqual(cancelledSlotModelsResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<CancelledSlotEntity>>>>()), Times.Once);
        }


        private IEnumerable<CancelledSlotEntity> DefaultCreateCancelledSlotEntities()
        {
            List<CancelledSlotEntity> cancelledSlotEntities = new List<CancelledSlotEntity>();

            var cancelledSlotEntity = new CancelledSlotEntity();
            cancelledSlotEntity.Id = Id;
            cancelledSlotEntity.Title = Title;
            cancelledSlotEntity.CreatedBy = CreatedBy;
            cancelledSlotEntity.CancelledBy = CancelledBy;
            cancelledSlotEntity.BookedBy = BookedBy;
            cancelledSlotEntity.TimeZone = TimeZone;
            cancelledSlotEntity.SlotDate = SlotDate;
            cancelledSlotEntity.SlotDateUtc = SlotDateUtc;
            cancelledSlotEntity.SlotStartTime = SlotStartTime;
            cancelledSlotEntity.SlotEndTime = SlotEndTime;
            cancelledSlotEntity.CreatedDateUtc = CreatedDateUtc;
            cancelledSlotEntities.Add(cancelledSlotEntity);

            cancelledSlotEntity = new CancelledSlotEntity();
            cancelledSlotEntity.Id = Id;
            cancelledSlotEntity.Title = Title;
            cancelledSlotEntity.CreatedBy = CreatedBy;
            cancelledSlotEntity.CancelledBy = CancelledBy;
            cancelledSlotEntity.BookedBy = BookedBy;
            cancelledSlotEntity.TimeZone = TimeZone;
            cancelledSlotEntity.SlotDate = SlotDate;
            cancelledSlotEntity.SlotDateUtc = SlotDateUtc;
            cancelledSlotEntity.SlotStartTime = SlotStartTime;
            cancelledSlotEntity.SlotEndTime = SlotEndTime;
            cancelledSlotEntity.CreatedDateUtc = CreatedDateUtc;
            cancelledSlotEntities.Add(cancelledSlotEntity);

            return cancelledSlotEntities;
        }


    }
}
