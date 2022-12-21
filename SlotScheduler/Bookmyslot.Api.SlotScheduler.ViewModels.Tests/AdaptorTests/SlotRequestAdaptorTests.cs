using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors;
using Bookmyslot.SharedKernel.Constants;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Tests.AdaptorTests
{
    [TestFixture]
    public class SlotRequestAdaptorTests
    {
        private const string Title = "Title";
        private const string Country = "Country";
        private static readonly DateTime date = new DateTime(2010, 1, 1);
        private readonly string ValidSlotDate = date.ToString(DateTimeConstants.ApplicationDatePattern);
        private const string IndiaTimeZone = TimeZoneConstants.IndianTimezone;
        private readonly TimeSpan SlotStartTime = new TimeSpan(1, 10, 10);
        private readonly TimeSpan SlotEndTime = new TimeSpan(2, 11, 11);
        private SlotRequestAdaptor slotRequestAdaptor;

        [SetUp]
        public void Setup()
        {
            this.slotRequestAdaptor = new SlotRequestAdaptor();
        }


        [Test]
        public void CreateSlotModel_ValidSlotViewModel_ReturnsValidSlotModel()
        {
            var slotViewModel = CreateDefaultSlotViewModel();

            var slotModel = this.slotRequestAdaptor.CreateSlotModel(slotViewModel);

            Assert.AreEqual(slotModel.Title, slotViewModel.Title);
            Assert.AreEqual(slotModel.Country, slotViewModel.Country);
            Assert.AreEqual(slotModel.SlotStartZonedDateTime.Zone.Id, slotViewModel.TimeZone);
            Assert.AreEqual(slotModel.SlotStartZonedDateTime.Day, date.Day);
            Assert.AreEqual(slotModel.SlotStartZonedDateTime.Month, date.Month);
            Assert.AreEqual(slotModel.SlotStartZonedDateTime.Year, date.Year);
            Assert.AreEqual(slotModel.SlotStartTime, slotViewModel.SlotStartTime);
            Assert.AreEqual(slotModel.SlotEndTime, slotViewModel.SlotEndTime);
        }


        private SlotViewModel CreateDefaultSlotViewModel()
        {
            var slotViewModel = new SlotViewModel();
            slotViewModel.Title = Title;
            slotViewModel.Country = Country;
            slotViewModel.SlotDate = ValidSlotDate;
            slotViewModel.TimeZone = IndiaTimeZone;
            slotViewModel.SlotStartTime = SlotStartTime;
            slotViewModel.SlotEndTime = SlotEndTime;

            return slotViewModel;
        }
    }
}
