using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business.Tests
{
    [TestFixture]
    public class CustomerBookedSlotBusinessTests
    {
        private const string CustomerId = "customerId";
        private const string CreatedBy1 = "CreatedBy1";
        private const string CreatedBy2 = "CreatedBy2";
        private const string CreatedBy3 = "CreatedBy3";
        private const string BookedBy1 = "BookedBy1";
        private const string BookedBy2 = "BookedBy2";
        private const string BookedBy3 = "BookedBy3";

        private CustomerBookedSlotBusiness customerBookedSlotBusiness;
        private Mock<ICustomerBookedSlotRepository> customerBookedSlotRepositoryMock;
        private Mock<ICustomerCancelledSlotRepository> customerCancelledSlotRepositoryMock;
        private Mock<ICustomerBusiness> customerBusinessMock;


        [SetUp]
        public void Setup()
        {
            customerBookedSlotRepositoryMock = new Mock<ICustomerBookedSlotRepository>();
            customerCancelledSlotRepositoryMock = new Mock<ICustomerCancelledSlotRepository>();
            customerBusinessMock = new Mock<ICustomerBusiness>();
            customerBookedSlotBusiness = new CustomerBookedSlotBusiness(customerBookedSlotRepositoryMock.Object, customerCancelledSlotRepositoryMock.Object, customerBusinessMock.Object);
        }


        [Test]
        public async Task GetCustomerBookedSlots_ValidCustomer_ReturnsCustomerSharedSlotModelSuccessResponse()
        {
            var slotModels = GetValidSlotModels();
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { Result = slotModels };
            customerBookedSlotRepositoryMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));
            Response<List<CustomerModel>> customerModelsMock = new Response<List<CustomerModel>>() { Result = new List<CustomerModel>() { GetValidCustomerModelByCustomerId(CreatedBy1), GetValidCustomerModelByCustomerId(CreatedBy2), GetValidCustomerModelByCustomerId(CreatedBy3) } };
            customerBusinessMock.Setup(a => a.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(customerModelsMock));

            var customerSharedSlotModelResponse = await this.customerBookedSlotBusiness.GetCustomerBookedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(customerSharedSlotModelResponse.Result.First().SlotModel);
            Assert.NotNull(customerSharedSlotModelResponse.Result.First().CreatedByCustomerModel);
            customerBookedSlotRepositoryMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerBookedSlots_InValidCustomer_ReturnsEmptyResponse()
        {
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
            customerBookedSlotRepositoryMock.Setup(a => a.GetCustomerBookedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSharedSlotModelResponse = await this.customerBookedSlotBusiness.GetCustomerBookedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Empty);
            customerBookedSlotRepositoryMock.Verify((m => m.GetCustomerBookedSlots(It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Never());
        }



        [Test]
        public async Task GetCustomerCompletedSlots_ValidCustomer_ReturnsCustomerSharedSlotModelSuccessResponse()
        {
            var slotModels = GetValidSlotModels();
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { Result = slotModels };
            customerBookedSlotRepositoryMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));
            Response<List<CustomerModel>> customerModelsMock = new Response<List<CustomerModel>>() { Result = new List<CustomerModel>() { GetValidCustomerModelByCustomerId(CreatedBy1), GetValidCustomerModelByCustomerId(CreatedBy2), GetValidCustomerModelByCustomerId(CreatedBy3) } };
            customerBusinessMock.Setup(a => a.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(customerModelsMock));

            var customerSharedSlotModelResponse = await this.customerBookedSlotBusiness.GetCustomerCompletedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(customerSharedSlotModelResponse.Result.First().SlotModel);
            Assert.NotNull(customerSharedSlotModelResponse.Result.First().CreatedByCustomerModel);
            customerBookedSlotRepositoryMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerCompletedSlots_InValidCustomer_ReturnsEmptyResponse()
        {
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
            customerBookedSlotRepositoryMock.Setup(a => a.GetCustomerCompletedSlots(It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSharedSlotModelResponse = await this.customerBookedSlotBusiness.GetCustomerCompletedSlots(CustomerId);

            Assert.AreEqual(customerSharedSlotModelResponse.ResultType, ResultType.Empty);
            customerBookedSlotRepositoryMock.Verify((m => m.GetCustomerCompletedSlots(It.IsAny<string>())), Times.Once());
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

        private CustomerModel GetValidCustomerModelByCustomerId(string customerId)
        {
            return new CustomerModel()
            {
                Id = customerId
            };
        }

    }
}