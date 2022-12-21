using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.ValueObject;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests
{
    [TestFixture]
    public class CustomerSharedSlotBusinessTests
    {
        private const string CustomerId = "customerId";
        private const string CreatedBy1 = "CreatedBy1";
        private const string CreatedBy2 = "CreatedBy2";
        private const string CreatedBy3 = "CreatedBy3";
        private const string BookedBy1 = "BookedBy1";
        private const string BookedBy2 = "BookedBy2";
        private const string BookedBy3 = "BookedBy3";

        private CustomerSharedSlotBusiness customerSharedSlotBusiness;
        private Mock<ICustomerSharedSlotRepository> customerSharedSlotRepositoryMock;
        private Mock<ICustomerCancelledSlotRepository> customerCancelledSlotRepositoryMock;
        private Mock<ICustomerBusiness> customerBusinessMock;


        [SetUp]
        public void Setup()
        {
            customerSharedSlotRepositoryMock = new Mock<ICustomerSharedSlotRepository>();
            customerCancelledSlotRepositoryMock = new Mock<ICustomerCancelledSlotRepository>();
            customerBusinessMock = new Mock<ICustomerBusiness>();
            customerSharedSlotBusiness = new CustomerSharedSlotBusiness(customerSharedSlotRepositoryMock.Object, customerCancelledSlotRepositoryMock.Object, customerBusinessMock.Object);
        }

        [Test]
        public async Task GetCustomerYetToBeBookedSlots_ValidCustomer_ReturnsCustomerSharedSlotModelSuccessResponse()
        {
            var slotModels = GetValidSlotModels();
            Result<IEnumerable<SlotModel>> slotModelResponseMock = new Result<IEnumerable<SlotModel>>() { Value = slotModels };
            customerSharedSlotRepositoryMock.Setup(a => a.GetCustomerYetToBeBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSharedSlotModelResponse = await this.customerSharedSlotBusiness.GetCustomerYetToBeBookedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(customerSharedSlotModelResponse.Value.SharedSlotModels.First().Value);
            customerSharedSlotRepositoryMock.Verify((m => m.GetCustomerYetToBeBookedSlots(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerYetToBeBookedSlots_InValidCustomer_ReturnsEmptyResponse()
        {
            Result<IEnumerable<SlotModel>> slotModelResponseMock = new Result<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
            customerSharedSlotRepositoryMock.Setup(a => a.GetCustomerYetToBeBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSharedSlotModelResponse = await this.customerSharedSlotBusiness.GetCustomerYetToBeBookedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Empty);
            customerSharedSlotRepositoryMock.Verify((m => m.GetCustomerYetToBeBookedSlots(It.IsAny<string>())), Times.Once());



        }


        [Test]
        public async Task GetCustomerBookedSlots_ValidCustomer_ReturnsCustomerSharedSlotModelSuccessResponse()
        {
            var slotModels = GetValidSlotModels();
            Result<IEnumerable<SlotModel>> slotModelResponseMock = new Result<IEnumerable<SlotModel>>() { Value = slotModels };
            customerSharedSlotRepositoryMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));
            Result<List<CustomerModel>> customerModelsMock = new Result<List<CustomerModel>>() { Value = new List<CustomerModel>() { GetValidCustomerModelByBookedByCustomerId(BookedBy1), GetValidCustomerModelByBookedByCustomerId(BookedBy2), GetValidCustomerModelByBookedByCustomerId(BookedBy3) } };
            customerBusinessMock.Setup(a => a.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(customerModelsMock));

            var customerSharedSlotModelResponse = await this.customerSharedSlotBusiness.GetCustomerBookedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(customerSharedSlotModelResponse.Value.SharedSlotModels.First().Value);
            Assert.NotNull(customerSharedSlotModelResponse.Value.SharedSlotModels.First().Key);
            customerSharedSlotRepositoryMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerBookedSlots_InValidCustomer_ReturnsEmptyResponse()
        {
            Result<IEnumerable<SlotModel>> slotModelResponseMock = new Result<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
            customerSharedSlotRepositoryMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSharedSlotModelResponse = await this.customerSharedSlotBusiness.GetCustomerBookedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Empty);
            customerSharedSlotRepositoryMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Never());
        }


        //[Test]
        //public async Task GetCustomerCancelledSlots_ValidCustomer_ReturnsCustomerSharedSlotModelSuccessResponse()
        //{
        //    var slotModels = GetValidSlotModels();
        //    Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { Result = slotModels };
        //    customerSharedSlotRepositoryMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

        //    var customerSharedSlotModelResponse = await this.customerSharedSlotBusiness.GetCustomerCancelledSlots(CustomerId);

        //    Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Success);
        //    Assert.NotNull(customerSharedSlotModelResponse.Result.First().SlotModel);
        //    customerSharedSlotRepositoryMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());
        //}

        //[Test]
        //public async Task GetCustomerCancelledSlots_InValidCustomer_ReturnsEmptyResponse()
        //{
        //    Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
        //    customerSharedSlotRepositoryMock.Setup(a => a.GetCustomerCancelledSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

        //    var customerSharedSlotModelResponse = await this.customerSharedSlotBusiness.GetCustomerCancelledSlots(CustomerId);

        //    Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Empty);
        //    customerSharedSlotRepositoryMock.Verify((m => m.GetCustomerCancelledSlots(It.IsAny<string>())), Times.Once());



        //}


        [Test]
        public async Task GetCustomerCompletedSlots_ValidCustomer_ReturnsCustomerSharedSlotModelSuccessResponse()
        {
            var slotModels = GetValidSlotModels();
            Result<IEnumerable<SlotModel>> slotModelResponseMock = new Result<IEnumerable<SlotModel>>() { Value = slotModels };
            customerSharedSlotRepositoryMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));
            Result<List<CustomerModel>> customerModelsMock = new Result<List<CustomerModel>>() { Value = new List<CustomerModel>() { GetValidCustomerModelByBookedByCustomerId(BookedBy1), GetValidCustomerModelByBookedByCustomerId(BookedBy2), GetValidCustomerModelByBookedByCustomerId(BookedBy3) } };
            customerBusinessMock.Setup(a => a.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(customerModelsMock));

            var customerSharedSlotModelResponse = await this.customerSharedSlotBusiness.GetCustomerCompletedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(customerSharedSlotModelResponse.Value.SharedSlotModels.First().Value);
            Assert.NotNull(customerSharedSlotModelResponse.Value.SharedSlotModels.First().Key);
            customerSharedSlotRepositoryMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerCompletedSlots_InValidCustomer_ReturnsEmptyResponse()
        {
            Result<IEnumerable<SlotModel>> slotModelResponseMock = new Result<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
            customerSharedSlotRepositoryMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSharedSlotModelResponse = await this.customerSharedSlotBusiness.GetCustomerCompletedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Empty);
            customerSharedSlotRepositoryMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Never());
        }

        private IEnumerable<SlotModel> GetValidSlotModels()
        {
            List<SlotModel> slotModels = new List<SlotModel>();

            var slotModel = new SlotModel();
            slotModel.CreatedBy = CreatedBy1;
            slotModel.BookedBy = BookedBy1;
            slotModels.Add(slotModel);

            slotModel = new SlotModel();
            slotModel.CreatedBy = CreatedBy2;
            slotModel.BookedBy = BookedBy2;
            slotModels.Add(slotModel);

            slotModel = new SlotModel();
            slotModel.CreatedBy = CreatedBy3;
            slotModel.BookedBy = BookedBy3;
            slotModels.Add(slotModel);

            return slotModels;
        }

        private CustomerModel GetValidCustomerModelByCreatedByCustomerId(string createdBy)
        {
            return new CustomerModel()
            {
                Id = createdBy,
            };
        }

        private CustomerModel GetValidCustomerModelByBookedByCustomerId(string bookedBy)
        {
            return new CustomerModel()
            {
                Id = bookedBy,
            };
        }

    }
}