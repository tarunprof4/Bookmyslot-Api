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
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.AdaptorTests
{
    [TestFixture]
    public class BookedSlotResponseAdaptorTests
    {
        private const string NoRecordsFound = "NoRecordsFound";
        private const string Country = "Country";
        private const string Title = "Title";
        private static readonly TimeSpan SlotStartTime = new TimeSpan(1, 1, 1);
        private static readonly TimeSpan SlotEndTime = new TimeSpan(2, 2, 2);
        private readonly TimeSpan SlotDuration = SlotEndTime - SlotStartTime;
        private const string SlotInformation = "SlotInformation";
        private static readonly DateTime date = new DateTime(2010, 1, 1);
        private readonly string ValidSlotDate = date.ToString(DateTimeConstants.ApplicationDatePattern);
        private static DateTime utcDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
        private readonly ZonedDateTime ValidSlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(utcDate, TimeZoneConstants.IndianTimezone);
        private const string IndiaTimeZone = TimeZoneConstants.IndianTimezone;

        private readonly ZonedDateTime MinZonedDateTime = new ZonedDateTime();
        private Mock<ISymmetryEncryption> symmetryEncryptionMock;
        private Mock<ICustomerResponseAdaptor> customerResponseAdaptorMock;
        private BookedSlotResponseAdaptor bookedSlotResponseAdaptor;

        [SetUp]
        public void Setup()
        {
            symmetryEncryptionMock = new Mock<ISymmetryEncryption>();
            customerResponseAdaptorMock = new Mock<ICustomerResponseAdaptor>();
            bookedSlotResponseAdaptor = new BookedSlotResponseAdaptor(symmetryEncryptionMock.Object, customerResponseAdaptorMock.Object);
        }


        [Test]
        public void CreateBookedSlotViewModel_EmptyBookAvailableSlotModel_ReturnsEmptyCreateBookedSlotViewModel()
        {
            var bookAvailableSlotViewModelResponse = bookedSlotResponseAdaptor.CreateBookedSlotViewModel(CreateEmptyDefaultBookedSlotModel());

            Assert.AreEqual(bookAvailableSlotViewModelResponse.ResultType, ResultType.Empty);
            Assert.AreEqual(bookAvailableSlotViewModelResponse.Messages[0], NoRecordsFound);
        }

        [Test]
        public void CreateBookedSlotViewModel_ValidBookAvailableSlotModelWithOutCustomerSettings_ReturnsValidBookedSlotViewModel()
        {
            CustomerViewModel customerViewModel = null;
            customerResponseAdaptorMock.Setup(a => a.CreateCustomerViewModel(It.IsAny<CustomerModel>())).Returns(customerViewModel);
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(SlotInformation);
            var bookedSlotModel = CreateDefaultBookedSlotModel();
            bookedSlotModel.Result.CustomerSettingsModel = null;

            var bookedSlotViewModelResponse = bookedSlotResponseAdaptor.CreateBookedSlotViewModel(bookedSlotModel);

            var bookedSlotViewModel = bookedSlotViewModelResponse.Result;
            Assert.AreEqual(bookedSlotViewModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(bookedSlotViewModel.BookedByCustomerCountry, string.Empty);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.Title, Title);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.Country, Country);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.SlotDuration, SlotDuration);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.SlotInformation, SlotInformation);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.SlotStartZonedDateTime, ValidSlotStartZonedDateTime);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.CustomerSlotStartZonedDateTime, MinZonedDateTime);
        }

        [Test]
        public void CreateBookedSlotViewModel_ValidBookAvailableSlotModelWithCustomerSettings_ReturnsValidCreateBookedSlotViewModel()
        {
            CustomerViewModel customerViewModel = null;
            customerResponseAdaptorMock.Setup(a => a.CreateCustomerViewModel(It.IsAny<CustomerModel>())).Returns(customerViewModel);
            symmetryEncryptionMock.Setup(a => a.Encrypt(It.IsAny<string>())).Returns(SlotInformation);
            var bookedSlotModel = CreateDefaultBookedSlotModel();

            var bookedSlotViewModelResponse = bookedSlotResponseAdaptor.CreateBookedSlotViewModel(bookedSlotModel);

            var bookedSlotViewModel = bookedSlotViewModelResponse.Result;
            Assert.AreEqual(bookedSlotViewModelResponse.ResultType, ResultType.Success);
            Assert.AreEqual(bookedSlotViewModel.BookedByCustomerCountry, Country);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.Title, Title);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.Country, Country);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.SlotDuration, SlotDuration);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.SlotInformation, SlotInformation);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.SlotStartZonedDateTime, ValidSlotStartZonedDateTime);
            Assert.AreEqual(bookedSlotViewModel.BookedSlotModels[0].Item2.CustomerSlotStartZonedDateTime, MinZonedDateTime);
        }

        private Response<BookedSlotModel> CreateEmptyDefaultBookedSlotModel()
        {
            var emptyBookedSlotModelResponse = new Response<BookedSlotModel>()
            {
                ResultType = ResultType.Empty,
                Messages = new List<string>() { NoRecordsFound }
            };

            return emptyBookedSlotModelResponse;
        }

        private Response<BookedSlotModel> CreateDefaultBookedSlotModel()
        {
            var bookedSlotModel = new BookedSlotModel();
            bookedSlotModel.CustomerSettingsModel = new CustomerSettingsModel() { Country = Country };
            bookedSlotModel.BookedSlotModels = new List<KeyValuePair<CustomerModel, SlotInforamtionInCustomerTimeZoneModel>>();
            bookedSlotModel.BookedSlotModels.Add(new KeyValuePair<CustomerModel, SlotInforamtionInCustomerTimeZoneModel>(new CustomerModel(), CreateDefaultSlotInformationInCustomerTimeZoneViewModel()));

            var bookedSlotModelResponse = new Response<BookedSlotModel>() { Result = bookedSlotModel };
            return bookedSlotModelResponse;
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
