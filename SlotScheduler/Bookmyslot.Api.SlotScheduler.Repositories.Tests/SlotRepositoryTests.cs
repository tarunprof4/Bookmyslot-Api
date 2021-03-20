﻿using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Tests
{

    public class SlotRepositoryTests
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

        private SlotRepository slotRepository;
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbInterceptor> dbInterceptorMock;

        [SetUp]
        public void SetUp()
        {
            dbConnectionMock = new Mock<IDbConnection>();
            dbInterceptorMock = new Mock<IDbInterceptor>();
            slotRepository = new SlotRepository(dbConnectionMock.Object, dbInterceptorMock.Object);
        }





        [Test]
        public async Task CreateSlot_ValidSlotModel_ReturnsSuccessResponse()
        {
            SlotModel slotModel = DefaultSlotModel();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var slotModelResponse = await slotRepository.CreateSlot(slotModel);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }

        [Test]
        public async Task DeleteSlot_ValidSlotModel_ReturnsSuccessResponse()
        {
            SlotModel slotModel = DefaultSlotModel();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var slotModelResponse = await slotRepository.DeleteSlot(DefaultSlotModel().Id);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelResponse.Result, true);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
        }


        [Test]
        public async Task GetSlot_NoRecordsFound_ReturnsEmptyResponse()
        {
            SlotEntity slotEntity = null;
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SlotEntity>>>())).Returns(Task.FromResult(slotEntity));

            var slotModelResponse = await slotRepository.GetSlot(Guid.NewGuid().ToString());

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Empty);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SlotEntity>>>()), Times.Once);
        }

        [Test]
        public async Task GetSlot_HasRecord_ReturnsSuccessResponse()
        {
            var slotEntity = DefaultCreateSlotEntities().ToList()[0];

            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SlotEntity>>>())).Returns(Task.FromResult(slotEntity));

            var slotModelResponse = await slotRepository.GetSlot(Guid.NewGuid().ToString());
            var slotModel = slotModelResponse.Result;
            Assert.AreEqual(slotModel.Id, Id);
            Assert.AreEqual(slotModel.Title, Title);
            Assert.AreEqual(slotModel.CreatedBy, CreatedBy);
            Assert.AreEqual(slotModel.BookedBy, BookedBy);
            Assert.AreEqual(slotModel.SlotStartZonedDateTime.Zone.Id, TimeZone);
            Assert.AreEqual(slotModel.SlotStartTime, SlotStartTime);
            Assert.AreEqual(slotModel.SlotEndTime, SlotEndTime);
            Assert.AreEqual(slotModel.CreatedDateUtc, CreatedDateUtc);
            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Success);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<SlotEntity>>>()), Times.Once);
        }


        [Test]
        public async Task UpdateSlot_ValidSlotModel_ReturnsSuccessResponse()
        {
            SlotModel slotModel = DefaultSlotModel();
            dbInterceptorMock.Setup(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>())).Returns(Task.FromResult(0));

            var slotModelResponse = await slotRepository.UpdateSlotBooking(slotModel.Id, slotModel.SlotMeetingLink, slotModel.BookedBy);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelResponse.Result, true);
            dbInterceptorMock.Verify(m => m.GetQueryResults(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<Task<int>>>()), Times.Once);
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

        private PageParameterModel DefaultPageParameterModel()
        {
            return new PageParameterModel();
        }

        private SlotModel DefaultSlotModel()
        {
            return new SlotModel() { Id = Id };
        }
    }
}
