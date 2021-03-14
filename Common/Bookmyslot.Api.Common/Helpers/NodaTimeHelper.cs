using Bookmyslot.Api.Common.Contracts.Constants;
using NodaTime;
using System;

namespace Bookmyslot.Api.Common.Helpers
{

    public static class NodaTimeHelper
    {
        public static LocalDateTime ConvertDateStringToLocalDateTime(string dateString, TimeSpan timeSpan)
        {
            var inputDateString = dateString.Split(DateTimeConstants.DateDelimiter);
            var month = Convert.ToInt32(inputDateString[0]);
            var date = Convert.ToInt32(inputDateString[1]);
            var year = Convert.ToInt32(inputDateString[2]);



            return new LocalDateTime(year, month, date, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public static ZonedDateTime ConvertLocalDateTimeToZonedDateTime(LocalDateTime localDateTime, string timeZone)
        {
            var dateTimeZone = DateTimeZoneProviders.Tzdb[timeZone];
            var zoneDateTime = dateTimeZone.AtStrictly(localDateTime);

            return zoneDateTime;
        }

        public static DateTime ConvertZonedDateTimeToUtcDateTime(ZonedDateTime zonedDateTime)
        {
            var utcZoneTime = zonedDateTime.ToInstant().InUtc().ToDateTimeUtc();
            return utcZoneTime;
        }

        public static ZonedDateTime ConvertUtcDateTimeToZonedDateTime(DateTime utcDateTime, string timeZone)
        {
            var nodaUtcTime = Instant.FromDateTimeUtc(DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc));
            var dateTimeZone = DateTimeZoneProviders.Tzdb[timeZone];
            var zoneDateTime = nodaUtcTime.InZone(dateTimeZone);
            return zoneDateTime;
        }

        public static ZonedDateTime GetCurrentUtcZonedDateTime()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            return now.InUtc();
        }

    }
}
