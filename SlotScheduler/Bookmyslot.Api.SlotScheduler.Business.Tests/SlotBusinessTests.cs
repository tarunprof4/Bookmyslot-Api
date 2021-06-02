using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Domain.Constants;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests
{
    [TestFixture]
    public class SlotBusinessTests
    {
        private readonly string SlotId = Guid.NewGuid().ToString();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private const string BookedBy = "BookedBy";
        private const string deletedBy = "deletedBy";
        private readonly DateTime ValidSlotDate = DateTime.UtcNow.AddDays(2);
        private readonly string InValidSlot = "InValidSlot";
        private readonly DateTime InValidSlotDate = DateTime.UtcNow.AddDays(-2);
        private readonly TimeSpan ValidSlotStartTime = new TimeSpan(0, 0, 0);
        private readonly TimeSpan ValidSlotEndTime = new TimeSpan(0, SlotConstants.MinimumSlotDuration, 0);

        private SlotBusiness slotBusiness;
        private Mock<ISlotRepository> slotRepositoryMock;
        private Mock<ICustomerCancelledSlotRepository> customerCancelledSlotRepositoryMock;
        private Mock<ICustomerLastSharedSlotBusiness> customerLastBookedSlotBusinessMock;
        private Mock<IValidator<SlotModel>> slotModelValidatorMock;

        [SetUp]
        public void Setup()
        {
            slotRepositoryMock = new Mock<ISlotRepository>();
            customerCancelledSlotRepositoryMock = new Mock<ICustomerCancelledSlotRepository>();
            customerLastBookedSlotBusinessMock = new Mock<ICustomerLastSharedSlotBusiness>();
            slotModelValidatorMock = new Mock<IValidator<SlotModel>>();
            slotBusiness = new SlotBusiness(slotRepositoryMock.Object, customerCancelledSlotRepositoryMock.Object,
                customerLastBookedSlotBusinessMock.Object, slotModelValidatorMock.Object);
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
            var slotModelResponse = await this.slotBusiness.GetSlot(It.IsAny<string>());


            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Messages.First(), AppBusinessMessagesConstants.SlotIdInvalid);
            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Never());
        }

        [Test]
        public async Task CreateSlot_ValidSlotDetails_ReturnsSlotCreatedSuccessResponse()
        {
            slotModelValidatorMock.Setup(a => a.Validate(It.IsAny<SlotModel>())).Returns(new ValidationResult());
            var slotModel = CreateValidSlotModel();
            Response<string> slotModelResponseMock = new Response<string>() { Result = slotModel.Id };
            slotRepositoryMock.Setup(a => a.CreateSlot(slotModel)).Returns(Task.FromResult(slotModelResponseMock));

            var slotModelResponse = await this.slotBusiness.CreateSlot(slotModel, slotModel.CreatedBy);

            Assert.AreEqual(slotModelResponseMock.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelResponseMock.Result, SlotId);

            slotRepositoryMock.Verify((m => m.CreateSlot(It.IsAny<SlotModel>())), Times.Once());
            customerLastBookedSlotBusinessMock.Verify((m => m.SaveCustomerLatestSharedSlot(It.IsAny<CustomerLastSharedSlotModel>())), Times.Once());
            slotModelValidatorMock.Verify((m => m.Validate(It.IsAny<SlotModel>())), Times.Once());
        }


        [Test]
        public async Task CreateSlot_InValidSlotDetails_ReturnsSlotValidationResponse()
        {
            List<ValidationFailure> validationFailures = CreateDefaultValidationFailure();
            slotModelValidatorMock.Setup(a => a.Validate(It.IsAny<SlotModel>())).Returns(new ValidationResult(validationFailures));
            var slotModel = CreateInvalidSlotModel();
            Response<string> slotModelResponseMock = new Response<string>() { Result = slotModel.Id };
            slotRepositoryMock.Setup(a => a.CreateSlot(slotModel)).Returns(Task.FromResult(slotModelResponseMock));

            var slotModelResponse = await this.slotBusiness.CreateSlot(slotModel, slotModel.CreatedBy);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.IsNull(slotModelResponse.Result);
            Assert.IsTrue(slotModelResponse.Messages.Contains(InValidSlot));
            slotRepositoryMock.Verify((m => m.CreateSlot(slotModel)), Times.Never());
            customerLastBookedSlotBusinessMock.Verify((m => m.SaveCustomerLatestSharedSlot(It.IsAny<CustomerLastSharedSlotModel>())), Times.Never());
            slotModelValidatorMock.Verify((m => m.Validate(It.IsAny<SlotModel>())), Times.Once());
        }

    

        [Test]
        public async Task CancelSlot_CancelledByCreatedBy_ReturnsSlotDeletedSuccessfully()
        {
            var slotModel = CreateValidBookedSlotModel();
            Response<bool> slotModelDeleteResponseMock = new Response<bool>() { Result = true };
            slotRepositoryMock.Setup(a => a.DeleteSlot(slotModel.Id)).Returns(Task.FromResult(slotModelDeleteResponseMock));
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { Result = slotModel };
            slotRepositoryMock.Setup(a => a.GetSlot(slotModel.Id)).Returns(Task.FromResult(slotModelGetResponseMock));
            Response<bool> customerCreateCancelledSlotMock = new Response<bool>() { Result = true };
            customerCancelledSlotRepositoryMock.Setup(a => a.CreateCustomerCancelledSlot(new CancelledSlotModel())).Returns(Task.FromResult(customerCreateCancelledSlotMock));

            var slotModelResponse = await this.slotBusiness.CancelSlot(slotModel.Id, slotModel.CreatedBy);

            Assert.AreEqual(slotModelDeleteResponseMock.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelDeleteResponseMock.Result, true);
            Assert.IsTrue(slotModel.BookedBy.Contains(slotModel.BookedBy));

            slotRepositoryMock.Verify((m => m.GetSlot(It.IsAny<string>())), Times.Once());
            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Never());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<string>())), Times.Once());
            customerCancelledSlotRepositoryMock.Verify((m => m.CreateCustomerCancelledSlot(It.IsAny<CancelledSlotModel>())), Times.Once());
        }


        [Test]
        public async Task CancelSlot_CancelledByBookedBy_ReturnsSlotUpdatedSuccessfully()
        {
            var slotModel = CreateValidBookedSlotModel();
            Response<bool> slotModelDeleteResponseMock = new Response<bool>() { Result = true };
            slotRepositoryMock.Setup(a => a.DeleteSlot(slotModel.Id)).Returns(Task.FromResult(slotModelDeleteResponseMock));
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { Result = slotModel };
            slotRepositoryMock.Setup(a => a.GetSlot(slotModel.Id)).Returns(Task.FromResult(slotModelGetResponseMock));
            Response<bool> customerCreateCancelledSlotMock = new Response<bool>() { Result = true };
            customerCancelledSlotRepositoryMock.Setup(a => a.CreateCustomerCancelledSlot(new CancelledSlotModel())).Returns(Task.FromResult(customerCreateCancelledSlotMock));

            var slotModelResponse = await this.slotBusiness.CancelSlot(slotModel.Id, slotModel.BookedBy);

            Assert.AreEqual(slotModelDeleteResponseMock.ResultType, ResultType.Success);
            Assert.AreEqual(slotModelDeleteResponseMock.Result, true);
            Assert.AreEqual(slotModel.BookedBy, string.Empty);

            slotRepositoryMock.Verify((m => m.GetSlot(It.IsAny<string>())), Times.Once());
            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Once());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<string>())), Times.Never());
            customerCancelledSlotRepositoryMock.Verify((m => m.CreateCustomerCancelledSlot(It.IsAny<CancelledSlotModel>())), Times.Once());
        }

        [Test]
        public async Task CancelSlot_SlotIdInvalid_ReturnsSlotValidationResponse()
        {
            var slotModelResponse = await this.slotBusiness.CancelSlot(string.Empty, deletedBy);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(slotModelResponse.Result, false);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessagesConstants.SlotIdInvalid));

            slotRepositoryMock.Verify((m => m.GetSlot(SlotId)), Times.Never());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<string>())), Times.Never());
            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Never());
            customerCancelledSlotRepositoryMock.Verify((m => m.CreateCustomerCancelledSlot(It.IsAny<CancelledSlotModel>())), Times.Never());

        }

        [Test]
        public async Task CancelSlot_SlotDoesntExists_ReturnsSlotNotFoundResponse()
        {
            var slotModel = new SlotModel();
            Response<SlotModel> slotModelGetResponseMock = new Response<SlotModel>() { ResultType = ResultType.Empty };
            slotRepositoryMock.Setup(a => a.GetSlot(It.IsAny<string>())).Returns(Task.FromResult(slotModelGetResponseMock));

            var slotModelResponse = await this.slotBusiness.CancelSlot(Guid.NewGuid().ToString(), deletedBy);

            Assert.AreEqual(slotModelResponse.ResultType, ResultType.Empty);
            Assert.IsTrue(slotModelResponse.Messages.Contains(AppBusinessMessagesConstants.SlotIdDoesNotExists));
            slotRepositoryMock.Verify((m => m.GetSlot(It.IsAny<string>())), Times.Once());
            slotRepositoryMock.Verify((m => m.DeleteSlot(It.IsAny<string>())), Times.Never());
            slotRepositoryMock.Verify((m => m.UpdateSlotBooking(It.IsAny<SlotModel>())), Times.Never());
            customerCancelledSlotRepositoryMock.Verify((m => m.CreateCustomerCancelledSlot(It.IsAny<CancelledSlotModel>())), Times.Never());
        }


        private SlotModel CreateValidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = SlotId;
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);
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
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(InValidSlotDate, TimeZoneConstants.IndianTimezone);
            slotModel.SlotStartTime = new TimeSpan(23, 0, 0);
            return slotModel;
        }

    

        private List<ValidationFailure> CreateDefaultValidationFailure()
        {
            ValidationFailure validationFailure = new ValidationFailure("", InValidSlot);
            List<ValidationFailure> validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            return validationFailures;
        }
    }
}