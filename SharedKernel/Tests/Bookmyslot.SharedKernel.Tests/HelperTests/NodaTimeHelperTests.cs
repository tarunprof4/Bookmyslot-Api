using Bookmyslot.SharedKernel.Constants;
using Bookmyslot.SharedKernel.Helpers;
using NodaTime;
using NUnit.Framework;
using System;

namespace Bookmyslot.SharedKernel.Tests.HelperTests
{
    public class NodaTimeHelperTests
    {

        private const string UtcTimeZone = "UTC";
        [SetUp]
        public void Setup()
        {
        }


        [TestCase("01-01-2000", TimeZoneConstants.IndianTimezone)]
        [TestCase("03-31-2021", TimeZoneConstants.LondonTimezone)]
        public void ConvertDateStringToZonedDateTime_PassedTimeZone_ReturnsRespectiveZonedDateTime(string dateString, string timeZone)
        {
            var zonedDateTime = NodaTimeHelper.ConvertDateStringToZonedDateTime(dateString, DateTimeConstants.ApplicationDatePattern, new TimeSpan(), timeZone);

            Assert.AreEqual(zonedDateTime.Zone.Id, timeZone);
            Assert.AreEqual(zonedDateTime.Date, zonedDateTime.Date);
            Assert.AreEqual(zonedDateTime.Month, zonedDateTime.Month);
            Assert.AreEqual(zonedDateTime.Year, zonedDateTime.Year);
        }




        [TestCase("01-01-2000", TimeZoneConstants.IndianTimezone)]
        [TestCase("03-31-2021", TimeZoneConstants.LondonTimezone)]
        [TestCase("12-31-2020", TimeZoneConstants.LondonTimezone)]
        public void ConvertLocalDateTimeToZonedDateTime_PassedTimeZone_ReturnsRespectiveZonedDateTime(string dateString, string timeZone)
        {
            var localDateTime = NodaTimeHelper.ConvertDateStringToLocalDateTime(dateString, DateTimeConstants.ApplicationDatePattern, new TimeSpan());
            var zonedDateTime = NodaTimeHelper.ConvertLocalDateTimeToZonedDateTime(localDateTime, timeZone);

            Assert.AreEqual(zonedDateTime.Zone.Id, timeZone);
            Assert.AreEqual(localDateTime.Date, zonedDateTime.Date);
            Assert.AreEqual(localDateTime.Month, zonedDateTime.Month);
            Assert.AreEqual(localDateTime.Year, zonedDateTime.Year);
        }


        [TestCase("01-01-2000", TimeZoneConstants.IndianTimezone, 18, 30, 0)]
        [TestCase("12-12-2000", TimeZoneConstants.IndianTimezone, 18, 30, 0)]
        public void ConvertZonedDateTimeToUtcDateTime_PassIndiaTimeZone_ReturnsUtcZonedDateTime(string dateString, string timeZone, int utcHour, int utcMinute, int utcSecond)
        {
            var localDateTime = NodaTimeHelper.ConvertDateStringToLocalDateTime(dateString, DateTimeConstants.ApplicationDatePattern, new TimeSpan());
            var indiaZonedDateTime = NodaTimeHelper.ConvertLocalDateTimeToZonedDateTime(localDateTime, timeZone);
            var utcDateTime = NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(indiaZonedDateTime);

            Assert.AreEqual(utcDateTime.Hour, utcHour);
            Assert.AreEqual(utcDateTime.Minute, utcMinute);
            Assert.AreEqual(utcDateTime.Second, utcSecond);
        }


        [TestCase(TimeZoneConstants.IndianTimezone, 19800)]
        [TestCase(TimeZoneConstants.LondonTimezone, 3600)]
        public void ConvertUtcDateTimeToZonedDateTime_PassedTimeZone_ReturnsRespectiveZonedDateTime(string timeZone, int offsetSeconds)
        {
            var utcDateTime = DateTime.UtcNow;
            var zonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(utcDateTime, timeZone);

            Assert.AreEqual(zonedDateTime.Zone.Id, timeZone);
            Assert.AreEqual(zonedDateTime.Offset.Seconds, offsetSeconds);
        }


        [Test()]
        public void FormatLocalDate_PassedLocal_ReturnsFormattedLocalDate()
        {
            var localDate = new LocalDate(2000, 1, 1);
            var formattedDate = NodaTimeHelper.FormatLocalDate(localDate, DateTimeConstants.ApplicationDatePattern);

            Assert.AreEqual("01-01-2000", formattedDate);
        }

        [Test()]
        public void ConvertZonedDateTimeToZonedDateTime_PassSourceZonedDateTime_ReturnsDestinationZonedDateTime()
        {
            var dateString = "02-02-2000";
            var localDateTime = NodaTimeHelper.ConvertDateStringToLocalDateTime(dateString, DateTimeConstants.ApplicationDatePattern, new TimeSpan());
            var indiaZonedDateTime = NodaTimeHelper.ConvertLocalDateTimeToZonedDateTime(localDateTime, TimeZoneConstants.IndianTimezone);
            var londonZonedDateTime = NodaTimeHelper.ConvertZonedDateTimeToZonedDateTime(indiaZonedDateTime, TimeZoneConstants.LondonTimezone);

            Assert.AreEqual(TimeZoneConstants.LondonTimezone, londonZonedDateTime.Zone.Id);
            Assert.AreEqual(2000, londonZonedDateTime.Year);
            Assert.AreEqual(2, londonZonedDateTime.Month);
            Assert.AreEqual(1, londonZonedDateTime.Day);
        }


        [Test()]
        public void GetCurrentUtcZonedDateTime_ReturnsUtcZonedDateTime()
        {
            var zonedDateTime = NodaTimeHelper.GetCurrentUtcZonedDateTime();

            Assert.AreEqual(zonedDateTime.Zone.Id, UtcTimeZone);
        }



    }
}
