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
    public class CustomerSlotBusinessTests
    {
        private const int PageParameterModelPageNumber = 1;
        private const string CreatedBy1 = "a@gmail.com";
        private const string CreatedBy2 = "b@gmail.com";
        private const string CreatedBy3 = "c@gmail.com";
        private CustomerSlotBusiness customerSlotBusiness;
        private Mock<ICustomerSlotRepository> customerSlotRepositoryMock;
        private Mock<ICustomerBusiness> customerBusinessMock;
        

        [SetUp]
        public void Setup()
        {
            customerSlotRepositoryMock = new Mock<ICustomerSlotRepository>();
            customerBusinessMock = new Mock<ICustomerBusiness>();
            customerSlotBusiness = new CustomerSlotBusiness(customerSlotRepositoryMock.Object, customerBusinessMock.Object);
        }

        [Test]
        public async Task GetDistinctCustomersLatestSlot_ValidInput_ReturnsCustomerSlotModelSuccessResponse()
        {
            var pageParameterModel = GetValidPageParameterModel();
            var slotModels = GetValidSlotModels();
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { Result = slotModels };
            customerSlotRepositoryMock.Setup(a => a.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())).Returns(Task.FromResult(slotModelResponseMock));
            Response<CustomerModel> customerModelResponseMock1 = new Response<CustomerModel>() { Result = GetValidCustomerModelByEmail(CreatedBy1) };
            Response<CustomerModel> customerModelResponseMock2 = new Response<CustomerModel>() { Result = GetValidCustomerModelByEmail(CreatedBy2) };
            Response<CustomerModel> customerModelResponseMock3 = new Response<CustomerModel>() { Result = GetValidCustomerModelByEmail(CreatedBy3) };

            customerBusinessMock.SetupSequence(a => a.GetCustomer(It.IsAny<string>()))
                .Returns(Task.FromResult(customerModelResponseMock1))
                .Returns(Task.FromResult(customerModelResponseMock2))
                .Returns(Task.FromResult(customerModelResponseMock3));


            var customerSlotModelResponse =  await this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            
            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(customerSlotModelResponse.Result[0].SlotModels.First().CreatedBy, CreatedBy1);
            Assert.NotNull(customerSlotModelResponse.Result[0].SlotModels);
            Assert.NotNull(customerSlotModelResponse.Result[0].CustomerModel);
            customerSlotRepositoryMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomer(It.IsAny<string>())), Times.AtLeastOnce());
        }

        [Test]
        public async Task GetDistinctCustomersLatestSlot_NoRecordsFound_ReturnsEmptyResponse()
        {
            var pageParameterModel = GetValidPageParameterModel();
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
            customerSlotRepositoryMock.Setup(a => a.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSlotModelResponse = await this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);

            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.Empty);
            customerSlotRepositoryMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomer(It.IsAny<string>())), Times.Never());
        }



        [Test]
        public async Task GetCustomerAvailableSlots_ValidInput_ReturnsCustomerSlotModelSuccessResponse()
        {
            var pageParameterModel = GetValidPageParameterModel();
            var slotModels = GetValidSlotModels();
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { Result = slotModels };
            customerSlotRepositoryMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));
            Response<CustomerModel> customerModelResponseMock1 = new Response<CustomerModel>() { Result = GetValidCustomerModelByEmail(CreatedBy1) };
            customerBusinessMock.Setup(a => a.GetCustomer(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponseMock1));


            var customerSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, CreatedBy1);

            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(customerSlotModelResponse.Result[0].SlotModels.First().CreatedBy, CreatedBy1);
            Assert.NotNull(customerSlotModelResponse.Result[0].SlotModels);
            Assert.NotNull(customerSlotModelResponse.Result[0].CustomerModel);
            customerSlotRepositoryMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomer(It.IsAny<string>())), Times.Once());
        }

        [Test]
        public async Task GetCustomerAvailableSlots_NoRecordsFound_ReturnsEmptyResponse()
        {
            var pageParameterModel = GetValidPageParameterModel();
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
            customerSlotRepositoryMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, string.Empty);

            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.Empty);
            customerSlotRepositoryMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomer(It.IsAny<string>())), Times.Never());
        }

        private PageParameterModel GetValidPageParameterModel()
        {
            return new PageParameterModel()
            {
                PageNumber = PageParameterModelPageNumber,
            };
        }

        private IEnumerable<SlotModel> GetValidSlotModels()
        {
            List<SlotModel> slotModels = new List<SlotModel>();
            
            var slotModel = new SlotModel();
            slotModel.CreatedBy = CreatedBy1;
            slotModels.Add(slotModel);

            slotModel = new SlotModel();
            slotModel.CreatedBy = CreatedBy2;
            slotModels.Add(slotModel);

            slotModel = new SlotModel();
            slotModel.CreatedBy = CreatedBy3;
            slotModels.Add(slotModel);

            return slotModels;
        }

        private CustomerModel GetValidCustomerModelByEmail(string email)
        {
            return new CustomerModel()
            {
                Email = email
            };
        }

    }
}