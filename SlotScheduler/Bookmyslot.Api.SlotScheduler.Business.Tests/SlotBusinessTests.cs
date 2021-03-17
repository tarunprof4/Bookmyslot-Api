using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests
{
    [TestFixture]
    public class SlotBusinessTests
    {
        private Guid SlotId = Guid.NewGuid();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private const string BookedBy = "BookedBy";
        private const string deletedBy = "deletedBy";
        private readonly DateTime ValidSlotDate = DateTime.UtcNow.AddDays(2);
        private readonly DateTime InValidSlotDate = DateTime.UtcNow.AddDays(-2);
        private readonly TimeSpan ValidSlotStartTime = new TimeSpan(0, 0, 0);
        private readonly TimeSpan ValidSlotEndTime = new TimeSpan(0, SlotConstants.MinimumSlotDuration, 0);

        private SlotBusiness slotBusiness;
        private Mock<ISlotRepository> slotRepositoryMock;
        private Mock<ICustomerCancelledSlotRepository> customerCancelledSlotRepositoryMock;

        [SetUp]
        public void Setup()
        {
            slotRepositoryMock = new Mock<ISlotRepository>();
            customerCancelledSlotRepositoryMock = new Mock<ICustomerCancelledSlotRepository>();
            slotBusiness = new SlotBusiness(slotRepositoryMock.Object, customerCancelledSlotRepositoryMock.Object);
        }

        [Test]
        public async Task GetSlot_ValidSlot_ReturnsSlotSuccessResponse()
        {
            await this.slotBusiness.GetSlot(SlotId);

            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Once());
        }

        [Test]
        public async Task GetSlot_InvalidSlotId_ReturnsSlotFailedResponse()
        {
            var slotModelResponse = await this.slotBusiness.GetSlot(It.IsAny<Guid>());


            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Messages.First(), AppBusinessMessagesConstants.SlotIdInvalid);
            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Never());
        }

        [Test]
        public async Task CreateSlot_ValidSlotDetails_ReturnsSlotCreatedSuccessResponse()
        {
            var slotModel = CreateValidSlotModel();
            Response<Guid> slotModelResponseMock = new Response<Guid>() { Result = slotModel.Id };
            slotRepositoryMock.Setup(a => a.CreateSlot(slotModel)).Returns(Task.FromResult(slotModelResponseMock));

            var slotModelResponse = await this.slotBusiness.CreateSlot(slotModel, slotModel.CreatedBy);

            Assert.AreEqual(slotModelResponseMock.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelResponseMock.Result, SlotId);

            slotRepositoryMock.Verify((m => m.CreateSlot(slotModel)), Times.Once());
        }


        [Test]
        public async Task CreateSlot_InValidSlotDetails_ReturnsSlotValidationResponse()
        {
            var slotModel = CreateInvalidSlotModel();
            Response<Guid> slotModelResponseMock = new Response<Guid>() { Result = slotModel.Id };
            slotRepositoryMock.Setup(a => a.CreateSlot(slotModel)).Returns(Task.FromResult(slotModelResponseMock));

            var slotModelResponse = await this.slotBusiness.CreateSlot(slotModel, slotModel.CreatedBy);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Result, Guid.Empty);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessagesConstants.InValidSlotDate));
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessagesConstants.SlotEndTimeInvalid));
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessagesConstants.SlotDurationInvalid));


            slotRepositoryMock.Verify((m => m.CreateSlot(slotModel)), Times.Never());
        }


        [Test]
        public async Task CancelSlot_CancelledByCreatedBy_ReturnsSlotDeletedSuccessfully()
        {
            var slotModel = CreateValidBookedSlotModel();
            slotModel.CreatedBy = deletedBy;
            Response<bool> slotModelDeleteResponseMock = new Response<bool>() { Result = true };
            slotRepositoryMock.Setup(a => a.DeleteSlot(slotModel.Id)).Returns(Task.FromResult(slotModelDeleteResponseMock));
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { Result = slotModel };
            slotRepositoryMock.Setup(a => a.GetSlot(slotModel.Id)).Returns(Task.FromResult(slotModelGetResponseMock));
            Response<bool> customerCreateCancelledSlotMock = new Response<bool>() { Result = true };
            customerCancelledSlotRepositoryMock.Setup(a => a.CreateCustomerCancelledSlot(new CancelledSlotModel())).Returns(Task.FromResult(customerCreateCancelledSlotMock));

            var slotModelResponse = await this.slotBusiness.CancelSlot(slotModel.Id, deletedBy);

            Assert.AreEqual(slotModelDeleteResponseMock.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelDeleteResponseMock.Result, true);
            Assert.IsTrue(slotModel.BookedBy.Contains(slotModel.BookedBy));

            slotRepositoryMock.Verify((m => m.GetSlot(It.IsAny<Guid>())), Times.Once());
            slotRepositoryMock.Verify((m => m.UpdateSlot(It.IsAny<Guid>(), It.IsAny<string>())), Times.Never());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<Guid>())), Times.Once());
            customerCancelledSlotRepositoryMock.Verify((m => m.CreateCustomerCancelledSlot(It.IsAny<CancelledSlotModel>())), Times.Once());
        }


        [Test]
        public async Task CancelSlot_CancelledByBookedBy_ReturnsSlotDeletedSuccessfully()
        {
            var slotModel = CreateValidBookedSlotModel();
            Response<bool> slotModelDeleteResponseMock = new Response<bool>() { Result = true };
            slotRepositoryMock.Setup(a => a.DeleteSlot(slotModel.Id)).Returns(Task.FromResult(slotModelDeleteResponseMock));
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { Result = slotModel };
            slotRepositoryMock.Setup(a => a.GetSlot(slotModel.Id)).Returns(Task.FromResult(slotModelGetResponseMock));
            Response<bool> customerCreateCancelledSlotMock = new Response<bool>() { Result = true };
            customerCancelledSlotRepositoryMock.Setup(a => a.CreateCustomerCancelledSlot(new CancelledSlotModel())).Returns(Task.FromResult(customerCreateCancelledSlotMock));

            var slotModelResponse = await this.slotBusiness.CancelSlot(slotModel.Id, deletedBy);

            Assert.AreEqual(slotModelDeleteResponseMock.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelDeleteResponseMock.Result, true);
            Assert.AreEqual(slotModel.BookedBy, string.Empty);

            slotRepositoryMock.Verify((m => m.GetSlot(It.IsAny<Guid>())), Times.Once());
            slotRepositoryMock.Verify((m => m.UpdateSlot(It.IsAny<Guid>(), It.IsAny<string>())), Times.Once());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<Guid>())), Times.Never());
            customerCancelledSlotRepositoryMock.Verify((m => m.CreateCustomerCancelledSlot(It.IsAny<CancelledSlotModel>())), Times.Once());
        }

        [Test]
        public async Task CancelSlot_SlotIdInvalid_ReturnsSlotValidationResponse()
        {
            var slotModelResponse = await this.slotBusiness.CancelSlot(Guid.Empty, deletedBy);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Result, false);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessagesConstants.SlotIdInvalid));

            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Never());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<Guid>())), Times.Never());
            slotRepositoryMock.Verify((m => m.UpdateSlot(It.IsAny<Guid>(), It.IsAny<string>())), Times.Never());
            customerCancelledSlotRepositoryMock.Verify((m => m.CreateCustomerCancelledSlot(It.IsAny<CancelledSlotModel>())), Times.Never());

        }

        [Test]
        public async Task CancelSlot_SlotDoesntExists_ReturnsSlotNotFoundResponse()
        {
            var slotModel = new SlotModel();
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { ResultType = ResultType.Empty };
            slotRepositoryMock.Setup(a => a.GetSlot(It.IsAny<Guid>())).Returns(Task.FromResult(slotModelGetResponseMock));

            var slotModelResponse = await this.slotBusiness.CancelSlot(Guid.NewGuid(), deletedBy);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Empty);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessagesConstants.SlotIdDoesNotExists));
            slotRepositoryMock.Verify((m => m.GetSlot(It.IsAny<Guid>())), Times.Once());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<Guid>())), Times.Never());
            slotRepositoryMock.Verify((m => m.UpdateSlot(It.IsAny<Guid>(), It.IsAny<string>())), Times.Never());
            customerCancelledSlotRepositoryMock.Verify((m => m.CreateCustomerCancelledSlot(It.IsAny<CancelledSlotModel>())), Times.Never());
        }


        private SlotModel CreateValidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = SlotId;
            slotModel.SlotZonedDate = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);
            slotModel.Title = Title;
            slotModel.CreatedBy = CreatedBy;
            slotModel.SlotStartTime = ValidSlotStartTime;
            slotModel.SlotEndTime = ValidSlotEndTime;

            return slotModel;
        }

        private SlotModel CreateValidBookedSlotModel()
        {
            var slotModel = CreateValidSlotModel();
            slotModel.BookedBy = BookedBy;

            return slotModel;
        }

        private SlotModel CreateInvalidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.SlotZonedDate = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(InValidSlotDate, TimeZoneConstants.IndianTimezone);
            slotModel.SlotStartTime = new TimeSpan(23, 0, 0);
            return slotModel;
        }
    }
}