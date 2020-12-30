using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
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
        private readonly DateTime StartTime = DateTime.UtcNow.AddDays(1);
        private readonly DateTime EndTime = DateTime.UtcNow.AddDays(3);

        private SlotBusiness slotBusiness;
        private Mock<ISlotRepository> slotRepositoryMock;

        [SetUp]
        public void Setup()
        {
            slotRepositoryMock = new Mock<ISlotRepository>();
            slotBusiness = new SlotBusiness(slotRepositoryMock.Object);
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
            Assert.AreEqual(slotModelResponse.Messages.First(), AppBusinessMessages.SlotIdInvalid);
            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Never());
        }

        [Test]
        public async Task CreateSlot_ValidSlotDetails_ReturnsSlotCreatedSuccessResponse()
        {
            var slotModel = CreateSlotModel();
            Response<Guid> slotModelResponseMock = new Response<Guid>() { Result = slotModel.Id };
            slotRepositoryMock.Setup(a => a.CreateSlot(slotModel)).Returns(Task.FromResult(slotModelResponseMock));

            var slotModelResponse = await this.slotBusiness.CreateSlot(slotModel);

            Assert.AreEqual(slotModelResponseMock.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelResponseMock.Result, SlotId);

            slotRepositoryMock.Verify((m => m.CreateSlot(slotModel)), Times.Once());
        }

        [Test]
        public async Task CreateSlot_SlotDetailsMissing_ReturnsSlotValidationResponse()
        {
            var slotModelResponse = await this.slotBusiness.CreateSlot(null);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Result, Guid.Empty);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.SlotDetailsMissing));

            slotRepositoryMock.Verify((m => m.CreateSlot(null)), Times.Never());
        }

        [Test]
        public async Task CreateSlot_InValidSlotDetails_ReturnsSlotCreatedValidationResponse()
        {
            var slotModel = new SlotModel();
            Response<Guid> slotModelResponseMock = new Response<Guid>() { Result = slotModel.Id };
            slotRepositoryMock.Setup(a => a.CreateSlot(slotModel)).Returns(Task.FromResult(slotModelResponseMock));

            var slotModelResponse = await this.slotBusiness.CreateSlot(slotModel);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Result, Guid.Empty);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.UserIdMissing));
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.SlotStartDateInvalid));
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.SlotEndDateInvalid));

            slotRepositoryMock.Verify((m => m.CreateSlot(slotModel)), Times.Never());
        }

        [Test]
        public async Task UpdateSlot_ValidSlotDetails_ReturnsSlotUpdatedSuccessfully()
        {
            var slotModel = CreateSlotModel();
            Response<bool> slotModelUpdateResponseMock = new Response<bool>() { Result = true };
            slotRepositoryMock.Setup(a => a.UpdateSlot(slotModel)).Returns(Task.FromResult(slotModelUpdateResponseMock));
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { Result = slotModel };
            slotRepositoryMock.Setup(a => a.GetSlot(slotModel.Id)).Returns(Task.FromResult(slotModelGetResponseMock));

            var slotModelResponse = await this.slotBusiness.UpdateSlot(slotModel);

            Assert.AreEqual(slotModelUpdateResponseMock.ResultType, ResultType.Success);

            slotRepositoryMock.Verify((m => m.UpdateSlot(slotModel)), Times.Once());
            slotRepositoryMock.Verify((m => m.GetSlot(slotModel.Id)), Times.Once());
        }


        [Test]
        public async Task UpdateSlot_SlotDetailsMissing_ReturnsSlotValidationResponse()
        {
            var slotModelResponse = await this.slotBusiness.UpdateSlot(null);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.SlotDetailsMissing));

            slotRepositoryMock.Verify((m => m.UpdateSlot(null)), Times.Never());
            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Never());
        }

        [Test]
        public async Task UpdateSlot_InValidSlotDetails_ReturnsSlotCreatedValidationResponse()
        {
            var slotModel = new SlotModel();

            var slotModelResponse = await this.slotBusiness.UpdateSlot(slotModel);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Result, false);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.UserIdMissing));
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.SlotStartDateInvalid));

            slotRepositoryMock.Verify((m => m.UpdateSlot(null)), Times.Never());
            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Never());
        }

        [Test]
        public async Task UpdateSlot_SlotDoesntExists_ReturnsSlotNotFoundResponse()
        {
            var slotModel = CreateSlotModel();
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { ResultType = ResultType.Empty };
            slotRepositoryMock.Setup(a => a.GetSlot(It.IsAny<Guid>())).Returns(Task.FromResult(slotModelGetResponseMock));

            var slotModelResponse = await this.slotBusiness.UpdateSlot(slotModel);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Empty);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.SlotIdDoesNotExists));
            slotRepositoryMock.Verify((m => m.GetSlot(It.IsAny<Guid>())), Times.Once());
            slotRepositoryMock.Verify((m => m.UpdateSlot(It.IsAny<SlotModel>())), Times.Never());
        }

        [Test]
        public async Task DeleteSlot_ValidSlotId_ReturnsSlotDeletedSuccessfully()
        {
            var slotModel = CreateSlotModel();
            Response<bool> slotModelDeleteResponseMock = new Response<bool>() { Result = true };
            slotRepositoryMock.Setup(a => a.DeleteSlot(slotModel)).Returns(Task.FromResult(slotModelDeleteResponseMock));
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { Result = slotModel };
            slotRepositoryMock.Setup(a => a.GetSlot(slotModel.Id)).Returns(Task.FromResult(slotModelGetResponseMock));

            var slotModelResponse = await this.slotBusiness.DeleteSlot(slotModel.Id);

            Assert.AreEqual(slotModelDeleteResponseMock.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelDeleteResponseMock.Result, true);

            slotRepositoryMock.Verify((m => m.DeleteSlot(slotModel)), Times.Once());
            slotRepositoryMock.Verify((m => m.GetSlot(slotModel.Id)), Times.Once());
        }

        [Test]
        public async Task DeleteSlot_SlotIdInvalid_ReturnsSlotValidationResponse()
        {
            var slotModelResponse = await this.slotBusiness.DeleteSlot(Guid.Empty);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Result, false);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.SlotIdInvalid));

            slotRepositoryMock.Verify((m => m.DeleteSlot(null)), Times.Never());
            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Never());
        }

        [Test]
        public async Task DeleteSlot_SlotDoesntExists_ReturnsSlotNotFoundResponse()
        {
            var slotModel = new SlotModel();
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { ResultType = ResultType.Empty };
            slotRepositoryMock.Setup(a => a.GetSlot(It.IsAny<Guid>())).Returns(Task.FromResult(slotModelGetResponseMock));

            var slotModelResponse = await this.slotBusiness.DeleteSlot(Guid.NewGuid());

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Empty);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessages.SlotIdDoesNotExists));
            slotRepositoryMock.Verify((m => m.GetSlot(It.IsAny<Guid>())), Times.Once());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<SlotModel>())), Times.Never());
        }


        private SlotModel CreateSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = SlotId;
            slotModel.Title = Title;
            slotModel.CreatedBy = CreatedBy;
            slotModel.StartTime = StartTime;
            slotModel.EndTime = EndTime;

            return slotModel;
        }
    }
}