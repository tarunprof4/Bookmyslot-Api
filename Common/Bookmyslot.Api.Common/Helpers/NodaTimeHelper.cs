using Bookmyslot.Api.Common.Contracts.Constants;
using NodaTime;
using NodaTime.Text;
using System;
using System.Globalization;

namespace Bookmyslot.Api.Common.Helpers
{

    public static class NodaTimeHelper
    {
        public static LocalDateTime ConvertDateStringToLocalDateTime(string dateString, string datePattern, TimeSpan timeSpan)
        {
            var localDatePattern = LocalDatePattern.CreateWithInvariantCulture(datePattern);
            var locadDate = localDatePattern.Parse(dateString);

            return new LocalDateTime(locadDate.Value.Year, locadDate.Value.Month, locadDate.Value.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public static ZonedDateTime ConvertDateStringToZonedDateTime(string dateString, string datePattern, TimeSpan timeSpan, string timeZone)
        {
            var localDateTime = ConvertDateStringToLocalDateTime(dateString, datePattern, timeSpan);
            return ConvertLocalDateTimeToZonedDateTime(localDateTime, timeZone);
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
            var nodaUtcTime = Instant.FromDateTimeUtc(utcDateTime);
            var dateTimeZone = DateTimeZoneProviders.Tzdb[timeZone];
            var zoneDateTime = nodaUtcTime.InZone(dateTimeZone);
            return zoneDateTime;
        }

        public static ZonedDateTime GetCurrentUtcZonedDateTime()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            return now.InUtc();
        }

        public static string FormatLocalDate(LocalDate localDate, string datePattern)
        {
            var formattedDate = localDate.ToString(datePattern, CultureInfo.InvariantCulture);
            return formattedDate;
        }

    }
}
