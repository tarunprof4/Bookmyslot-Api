using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
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
        private const string CreatedBy1 = "CreatedBy1";
        private const string CreatedBy2 = "CreatedBy2";
        private const string CreatedBy3 = "CreatedBy3";
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
            Response<IEnumerable<string>> customersResponseMock = new Response<IEnumerable<string>>() { Result = GetCustomersWithSlots() };
            customerSlotRepositoryMock.Setup(a => a.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())).Returns(Task.FromResult(customersResponseMock));
            Response<List<CustomerModel>> customerModelsMock = new Response<List<CustomerModel>>() { Result = new List<CustomerModel>() { GetValidCustomerModelByCustomerId(CreatedBy1), GetValidCustomerModelByCustomerId(CreatedBy2), GetValidCustomerModelByCustomerId(CreatedBy3) } };
            customerBusinessMock.Setup(a => a.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(customerModelsMock));


            var customerSlotModelResponse = await this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);

            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(customerSlotModelResponse.Result[0].CustomerModel);
            customerSlotRepositoryMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomersByCustomerIds(It.IsAny<IEnumerable<string>>())), Times.Once());
        }

        [Test]
        public async Task GetDistinctCustomersLatestSlot_NoRecordsFound_ReturnsEmptyResponse()
        {
            var pageParameterModel = GetValidPageParameterModel();
            Response<IEnumerable<string>> customersResponseMock = new Response<IEnumerable<string>>() { ResultType = ResultType.Empty };
            customerSlotRepositoryMock.Setup(a => a.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())).Returns(Task.FromResult(customersResponseMock));

            var customerSlotModelResponse = await this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);

            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.Empty);
            customerSlotRepositoryMock.Verify((m => m.GetDistinctCustomersNearestSlotFromToday(It.IsAny<PageParameterModel>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomerById(It.IsAny<string>())), Times.Never());
        }



        [Test]
        public async Task GetCustomerAvailableSlots_ValidInput_ReturnsCustomerSlotModelSuccessResponse()
        {
            var pageParameterModel = GetValidPageParameterModel();
            var slotModels = GetValidSlotModels();
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { Result = slotModels };
            customerSlotRepositoryMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));
            Response<CustomerModel> customerModelResponseMock1 = new Response<CustomerModel>() { Result = GetValidCustomerModelByCustomerId(CreatedBy1) };
            customerBusinessMock.Setup(a => a.GetCustomerById(It.IsAny<string>())).Returns(Task.FromResult(customerModelResponseMock1));


            var customerSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, CreatedBy1);

            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.Success);
            Assert.NotNull(customerSlotModelResponse.Result.SlotModelsInforamtion);
            Assert.AreEqual(customerSlotModelResponse.Result.SlotModelsInforamtion[0].Key.CreatedBy, CreatedBy1);
            Assert.NotNull(customerSlotModelResponse.Result.CustomerModel);
            customerSlotRepositoryMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomerById(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task GetCustomerAvailableSlots_CustomerIdMissing_ReturnsValidationResponse()
        {
            var pageParameterModel = GetValidPageParameterModel();

            var customerSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, string.Empty);

            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.ValidationError);
            Assert.IsTrue(customerSlotModelResponse.Messages.Contains(AppBusinessMessagesConstants.CustomerIdNotValid));
            customerSlotRepositoryMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())), Times.Never());
            customerBusinessMock.Verify((m => m.GetCustomerById(It.IsAny<string>())), Times.Never());
        }

        [Test]
        public async Task GetCustomerAvailableSlots_NoRecordsFound_ReturnsEmptyResponse()
        {
            var pageParameterModel = GetValidPageParameterModel();
            Response<IEnumerable<SlotModel>> slotModelResponseMock = new Response<IEnumerable<SlotModel>>() { ResultType = ResultType.Empty };
            customerSlotRepositoryMock.Setup(a => a.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())).Returns(Task.FromResult(slotModelResponseMock));

            var customerSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, CreatedBy1);

            Assert.AreEqual(customerSlotModelResponse.ResultType, ResultType.Empty);
            customerSlotRepositoryMock.Verify((m => m.GetCustomerAvailableSlots(It.IsAny<PageParameterModel>(), It.IsAny<string>())), Times.Once());
            customerBusinessMock.Verify((m => m.GetCustomerById(It.IsAny<string>())), Times.Never());
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


        private IEnumerable<string> GetCustomersWithSlots()
        {
            List<string> customers = new List<string>();
            customers.Add(CreatedBy1);
            customers.Add(CreatedBy2);
            customers.Add(CreatedBy3);

            return customers;
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