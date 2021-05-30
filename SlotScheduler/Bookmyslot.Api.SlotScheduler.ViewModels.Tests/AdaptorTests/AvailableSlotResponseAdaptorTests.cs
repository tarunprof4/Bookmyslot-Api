using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Moq;
using NodaTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.AdaptorTests
{
    [TestFixture]
    public class AvailableSlotResponseAdaptorTests
    {
        private const string NoRecordsFound = "NoRecordsFound";
        private const string Country = "Country";
        private const string Title = "Title";
        private static readonly TimeSpan SlotStartTime = new TimeSpan(1, 1, 1);
        private static readonly TimeSpan SlotEndTime = new TimeSpan(2, 2, 2);
        private readonly TimeSpan SlotDuration = SlotEndTime - SlotStartTime;
        private const string SlotInformation = "SlotInformation";
        private static readonly DateTime date = new DateTime(2010, 1, 1);
        private static readonly DateTime utcDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
        private readonly ZonedDateTime ValidSlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(utcDate, TimeZoneConstants.IndianTimezone);
        private readonly ZonedDateTime MinZonedDateTime = new ZonedDateTime();

        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<ICustomerResponseAdaptor> customerResponseAdaptorMock;
        private AvailableSlotResponseAdaptor availableSlotResponseAdaptor;

        [SetUp]
        public void Setup()
        {
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            customerResponseAdaptorMock = new Mock<ICustomerResponseAdaptor>();
            availableSlotResponseAdaptor = new AvailableSlotResponseAdaptor(symmetryEncryptionMock.Object, customerResponseAdaptorMock.Object);
        }


        [Test]
        public void CreateBookAvailableSlotViewModel_EmptyBookAvailableSlotModel_ReturnsEmptyBookAvailableSlotViewModel()
        {
            var bookAvailableSlotViewModelResponse = availableSlotResponseAdaptor.CreateBookAvailableSlotViewModel(CreateEmptyDefaultBookAvailableSlotModel());

            Assert.AreEqual(bookAvailableSlotViewModelResponse.ResultType, ResultType.Empty);
            Assert.AreEqual(bookAvailableSlotViewModelResponse.Messages[0], NoRecordsFound);
        }

        [Test]
        public void CreateBookAvailableSlotViewModel_ValidBookAvailableSlotModelWithOutCustomerSettings_ReturnsValidBookAvailableSlotViewModel()
        {
            CustomerViewModel customerViewModel = null;
            customerResponseAdaptorMock.Setup(a => a.CreateCustomerViewModel(It.IsAny<CustomerModel>())).Returns(customerViewModel);
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(SlotInformation);
            var bookAvailableSlotModel = CreateDefaultBookAvailableSlotModel();
            bookAvailableSlotModel.Result.CustomerSettingsModel = null;

            var bookAvailableSlotViewModelResponse = availableSlotResponseAdaptor.CreateBookAvailableSlotViewModel(bookAvailableSlotModel);

            var bookAvailableSlotViewModel = bookAvailableSlotViewModelResponse.Result;
            Assert.AreEqual(bookAvailableSlotViewModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(bookAvailableSlotViewModel.ToBeBookedByCustomerCountry, string.Empty);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].Title, Title);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].Country, Country);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].SlotDuration, SlotDuration);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].SlotInformation, SlotInformation);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].SlotStartZonedDateTime, ValidSlotStartZonedDateTime);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].CustomerSlotStartZonedDateTime, MinZonedDateTime);
        }

        [Test]
        public void CreateBookAvailableSlotViewModel_ValidBookAvailableSlotModelWithCustomerSettings_ReturnsValidBookAvailableSlotViewModel()
        {
            CustomerViewModel customerViewModel = null;
            customerResponseAdaptorMock.Setup(a => a.CreateCustomerViewModel(It.IsAny<CustomerModel>())).Returns(customerViewModel);
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(SlotInformation);

            var bookAvailableSlotViewModelResponse = availableSlotResponseAdaptor.CreateBookAvailableSlotViewModel(CreateDefaultBookAvailableSlotModel());
            var bookAvailableSlotViewModel = bookAvailableSlotViewModelResponse.Result;

            Assert.AreEqual(bookAvailableSlotViewModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(bookAvailableSlotViewModel.ToBeBookedByCustomerCountry, Country);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].Title, Title);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].Country, Country);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].SlotDuration, SlotDuration);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].SlotInformation, SlotInformation);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].SlotStartZonedDateTime, ValidSlotStartZonedDateTime);
            Assert.AreEqual(bookAvailableSlotViewModel.BookAvailableSlotModels[0].CustomerSlotStartZonedDateTime, MinZonedDateTime);
        }

        private Response<BookAvailableSlotModel> CreateEmptyDefaultBookAvailableSlotModel()
        {
            var emptyBookAvailableSlotModelResponse = new Response<BookAvailableSlotModel>()
            {
                ResultType = ResultType.Empty,
                Messages = new List<string>() { NoRecordsFound }
            };

            return emptyBookAvailableSlotModelResponse;
        }

        private Response<BookAvailableSlotModel> CreateDefaultBookAvailableSlotModel()
        {
            var bookAvailableSlotModel = new BookAvailableSlotModel();
            bookAvailableSlotModel.CustomerSettingsModel = new CustomerSettingsModel() { Country = Country };
            bookAvailableSlotModel.AvailableSlotModels = new List<SlotInforamtionInCustomerTimeZoneModel>();
            bookAvailableSlotModel.AvailableSlotModels.Add(CreateDefaultSlotInformationInCustomerTimeZoneViewModel());

            var bookAvailableSlotModelResponse = new Response<BookAvailableSlotModel>() { Result = bookAvailableSlotModel };
            return bookAvailableSlotModelResponse;
        }

        private SlotInforamtionInCustomerTimeZoneModel CreateDefaultSlotInformationInCustomerTimeZoneViewModel()
        {
            return new SlotInforamtionInCustomerTimeZoneModel()
            {
                SlotModel = new SlotModel()
                {
                    Title = Title,
                    Country = Country,
                    SlotStartTime = SlotStartTime,
                    SlotEndTime = SlotEndTime,
                    SlotStartZonedDateTime = ValidSlotStartZonedDateTime
                }
            };
        }




    }
}
