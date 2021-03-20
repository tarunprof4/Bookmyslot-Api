using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Tests
{
    public class CustomerBookedSlotRepositoryTests
    {
        private const string CustomerId = "CustomerId";

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

        private CustomerBookedSlotRepository customerBookedSlotRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            customerBookedSlotRepository = new CustomerBookedSlotRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }

        [Test]
        public async Task GetCustomerBookedSlots_NoRecordsFound_ReturnsEmptyResponse()
        {
            IEnumerable<SlotEntity> slotEntities = new List<SlotEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SlotEntity>>>>())).Returns(Task.FromResult(slotEntities));

            var slotModelsResponse = await customerBookedSlotRepository.GetCustomerBookedSlots(CustomerId);

            Assert.AreEqual(slotModelsResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SlotEntity>>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerBookedSlots_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SlotEntity>>>>())).Returns(Task.FromResult(DefaultCreateSlotEntities()));

            var slotModelResponse = await customerBookedSlotRepository.GetCustomerBookedSlots(CustomerId);

            foreach (var slotModel in slotModelResponse.Result)
            {
                Assert.AreEqual(slotModel.Id, Id);
                Assert.AreEqual(slotModel.Title, Title);
                Assert.AreEqual(slotModel.CreatedBy, CreatedBy);
                Assert.AreEqual(slotModel.BookedBy, BookedBy);
                Assert.AreEqual(slotModel.SlotStartZonedDateTime.Zone.Id, TimeZone);
                Assert.AreEqual(slotModel.SlotStartTime, SlotStartTime);
                Assert.AreEqual(slotModel.SlotEndTime, SlotEndTime);
                Assert.AreEqual(slotModel.CreatedDateUtc, CreatedDateUtc);
            }
            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SlotEntity>>>>()), Times.Once);
        }



        [Test]
        public async Task GetCustomerCompletedSlots_NoRecordsFound_ReturnsEmptyResponse()
        {
            IEnumerable<SlotEntity> slotEntities = new List<SlotEntity>();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SlotEntity>>>>())).Returns(Task.FromResult(slotEntities));

            var slotModelsResponse = await customerBookedSlotRepository.GetCustomerCompletedSlots(CustomerId);

            Assert.AreEqual(slotModelsResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SlotEntity>>>>()), Times.Once);
        }

        [Test]
        public async Task GetCustomerCompletedSlots_HasRecord_ReturnsSuccessResponse()
        {
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SlotEntity>>>>())).Returns(Task.FromResult(DefaultCreateSlotEntities()));

            var slotModelResponse = await customerBookedSlotRepository.GetCustomerCompletedSlots(CustomerId);

            foreach (var slotModel in slotModelResponse.Result)
            {
                Assert.AreEqual(slotModel.Id, Id);
                Assert.AreEqual(slotModel.Title, Title);
                Assert.AreEqual(slotModel.CreatedBy, CreatedBy);
                Assert.AreEqual(slotModel.BookedBy, BookedBy);
                Assert.AreEqual(slotModel.SlotStartZonedDateTime.Zone.Id, TimeZone);
                Assert.AreEqual(slotModel.SlotStartTime, SlotStartTime);
                Assert.AreEqual(slotModel.SlotEndTime, SlotEndTime);
                Assert.AreEqual(slotModel.CreatedDateUtc, CreatedDateUtc);
            }
            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<IEnumerable<SlotEntity>>>>()), Times.Once);
        }






        private IEnumerable<SlotEntity> DefaultCreateSlotEntities()
        {
            List<SlotEntity> slotEntities = new List<SlotEntity>();

            var slotEntity = new SlotEntity();
            slotEntity.Id = Id;
            slotEntity.Title = Title;
            slotEntity.CreatedBy = CreatedBy;
            slotEntity.BookedBy = BookedBy;
            slotEntity.TimeZone = TimeZone;
            slotEntity.SlotDate = SlotDate;
            slotEntity.SlotStartDateTimeUtc = SlotDateUtc;
            slotEntity.SlotStartTime = SlotStartTime;
            slotEntity.SlotEndTime = SlotEndTime;
            slotEntity.CreatedDateUtc = CreatedDateUtc;
            slotEntities.Add(slotEntity);

            slotEntity = new SlotEntity();
            slotEntity.Id = Id;
            slotEntity.Title = Title;
            slotEntity.CreatedBy = CreatedBy;
            slotEntity.BookedBy = BookedBy;
            slotEntity.TimeZone = TimeZone;
            slotEntity.SlotDate = SlotDate;
            slotEntity.SlotStartDateTimeUtc = SlotDateUtc;
            slotEntity.SlotStartTime = SlotStartTime;
            slotEntity.SlotEndTime = SlotEndTime;
            slotEntity.CreatedDateUtc = CreatedDateUtc;
            slotEntities.Add(slotEntity);

            return slotEntities;
        }


    }
}