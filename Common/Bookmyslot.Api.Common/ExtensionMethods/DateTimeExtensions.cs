using System;

namespace Bookmyslot.Api.Common.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static DateTime GetDateTimeToUtcByTimeZone(this DateTime dateTime, string timeZome)
        {
            var newDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(newDate, TimeZoneInfo.FindSystemTimeZoneById(timeZome), TimeZoneInfo.Utc);
        }

        public static DateTime GetDateTimeByTimeZone(this DateTime dateTime, string timeZome, TimeSpan timeSpan)
        {
            var newDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(newDate, TimeZoneInfo.FindSystemTimeZoneById(timeZome), TimeZoneInfo.FindSystemTimeZoneById(timeZome));
        }

        public static DateTime GetDateTimeFromUtcToTimeZone(this DateTime dateTime, string timeZome)
        {
            var newDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(newDate, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById(timeZome));
        }

      
    }
}
