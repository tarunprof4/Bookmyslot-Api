using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.Common.Tests.HelperTests
{

    public class NodaTimeHelperTests
    {

        private const string UtcTimeZone = "UTC";
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("01-01-2000", TimeZoneConstants.IndianTimezone)]
        [TestCase("31-03-2021", TimeZoneConstants.LondonTimezone)]
        [TestCase("31-12-2020", TimeZoneConstants.LondonTimezone)]
        public void ConvertLocalDateTimeToZonedDateTime_PassedTimeZone_ReturnsRespectiveZonedDateTime(string dateString, string timeZone)
        {
            var localDateTime = NodaTimeHelper.ConvertDateStringToLocalDateTime(dateString, new TimeSpan());
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
            var localDateTime = NodaTimeHelper.ConvertDateStringToLocalDateTime(dateString, new TimeSpan());
            var indiaZonedDateTime = NodaTimeHelper.ConvertLocalDateTimeToZonedDateTime(localDateTime, timeZone);
            var utcDateTime = NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(indiaZonedDateTime);

            Assert.AreEqual(utcDateTime.Hour, utcHour);
            Assert.AreEqual(utcDateTime.Minute, utcMinute);
            Assert.AreEqual(utcDateTime.Second, utcSecond);
        }


        [TestCase(TimeZoneConstants.IndianTimezone, 19800)]
        [TestCase(TimeZoneConstants.LondonTimezone, 0)]
        public void ConvertUtcDateTimeToZonedDateTime_PassedTimeZone_ReturnsRespectiveZonedDateTime(string timeZone, int offsetSeconds)
        {
            var utcDateTime = DateTime.UtcNow;
            var zonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(utcDateTime, timeZone);

            Assert.AreEqual(zonedDateTime.Zone.Id, timeZone);
            Assert.AreEqual(zonedDateTime.Offset.Seconds, offsetSeconds);
        }


        public void GetCurrentUtcZonedDateTime_ReturnsUtcZonedDateTime()
        {
            var zonedDateTime = NodaTimeHelper.GetCurrentUtcZonedDateTime();

            Assert.AreEqual(zonedDateTime.Zone.Id, UtcTimeZone);
        }



    }
}
