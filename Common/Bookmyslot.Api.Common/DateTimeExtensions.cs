using System;

namespace Bookmyslot.Api.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime GetDateTimeUtcByTimeZone(this DateTime dateTime, string timeZome, TimeSpan timeSpan)
        {
            var newDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            return TimeZoneInfo.ConvertTime(newDate, TimeZoneInfo.FindSystemTimeZoneById(timeZome), TimeZoneInfo.Utc);
        }
    }
}
