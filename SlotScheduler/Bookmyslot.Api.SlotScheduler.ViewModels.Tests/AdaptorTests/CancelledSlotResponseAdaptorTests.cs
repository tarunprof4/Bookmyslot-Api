using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.SharedKernel.Constants;
using Bookmyslot.SharedKernel.Helpers;
using Moq;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.AdaptorTests
{


    [TestFixture]
    public class CancelledSlotResponseAdaptorTests
    {
        private const string Country = "Country";
        private const string Title = "Title";
        private static readonly TimeSpan SlotStartTime = new TimeSpan(1, 1, 1);
        private static readonly TimeSpan SlotEndTime = new TimeSpan(2, 2, 2);
        private static readonly DateTime date = new DateTime(2010, 1, 1);
        private static readonly DateTime utcDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
        private readonly ZonedDateTime ValidSlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(utcDate, TimeZoneConstants.IndianTimezone);
        private Mock<ICustomerResponseAdaptor> customerResponseAdaptorMock;
        private CancelledSlotResponseAdaptor cancelledSlotResponseAdaptor;

        [SetUp]
        public void Setup()
        {
            customerResponseAdaptorMock = new Mock<ICustomerResponseAdaptor>();
            cancelledSlotResponseAdaptor = new CancelledSlotResponseAdaptor(customerResponseAdaptorMock.Object);
        }


        [Test]
        public void CreateCancelledSlotViewModels_ValidCancelledSlotModels_ReturnsSuccessCancelledSlotViewModels()
        {
            var cancelledSlotViewModels = cancelledSlotResponseAdaptor.CreateCancelledSlotViewModels(CreateDefaultCancelledSlotModels());
            var cancelledSlotViewModelsList = cancelledSlotViewModels.ToList();

            Assert.AreEqual(cancelledSlotViewModelsList[0].Title, Title);
            Assert.AreEqual(cancelledSlotViewModelsList[0].Country, Country);
            Assert.AreEqual(cancelledSlotViewModelsList[0].SlotStartZonedDateTime, ValidSlotStartZonedDateTime);
            Assert.AreEqual(cancelledSlotViewModelsList[0].SlotStartTime, SlotStartTime);
            Assert.AreEqual(cancelledSlotViewModelsList[0].SlotEndTime, SlotEndTime);
        }



        [Test]
        public void CreateCancelledSlotInformationViewModels_ValidCancelledSlotInformationModels_ReturnsSuccessCancelledSlotInformationViewModels()
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();
            customerResponseAdaptorMock.Setup(a => a.CreateCustomerViewModel(It.IsAny<CustomerModel>())).Returns(customerViewModel);

            var cancelledSlotInformationViewModels = cancelledSlotResponseAdaptor.CreateCancelledSlotInformationViewModels(CreateDefaultCancelledSlotInformationModels());
            var cancelledSlotInformationViewModelsList = cancelledSlotInformationViewModels.ToList();

            Assert.IsNotNull(cancelledSlotInformationViewModelsList[0].CancelledByCustomerViewModel);
            Assert.AreEqual(cancelledSlotInformationViewModelsList[0].CancelledSlotViewModel.Title, Title);
            Assert.AreEqual(cancelledSlotInformationViewModelsList[0].CancelledSlotViewModel.Country, Country);
            Assert.AreEqual(cancelledSlotInformationViewModelsList[0].CancelledSlotViewModel.SlotStartZonedDateTime, ValidSlotStartZonedDateTime);
            Assert.AreEqual(cancelledSlotInformationViewModelsList[0].CancelledSlotViewModel.SlotStartTime, SlotStartTime);
            Assert.AreEqual(cancelledSlotInformationViewModelsList[0].CancelledSlotViewModel.SlotEndTime, SlotEndTime);

        }




        private CancelledSlotModel CreateDefaultCancelledSlotModel()
        {
            return new CancelledSlotModel()
            {
                Title = Title,
                Country = Country,
                SlotStartZonedDateTime = ValidSlotStartZonedDateTime,
                SlotStartTime = SlotStartTime,
                SlotEndTime = SlotEndTime,
            };
        }

        private IEnumerable<CancelledSlotModel> CreateDefaultCancelledSlotModels()
        {
            var cancelledSlotModels = new List<CancelledSlotModel>();
            cancelledSlotModels.Add(CreateDefaultCancelledSlotModel());
            return cancelledSlotModels;
        }


        private IEnumerable<CancelledSlotInformationModel> CreateDefaultCancelledSlotInformationModels()
        {
            var cancelledSlotInformationModel = new CancelledSlotInformationModel();
            cancelledSlotInformationModel.CancelledSlotModel = CreateDefaultCancelledSlotModel();

            var cancelledSlotInformationModels = new List<CancelledSlotInformationModel>();
            cancelledSlotInformationModels.Add(cancelledSlotInformationModel);

            return cancelledSlotInformationModels;
        }



    }
}
