using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime GetDateTimeByTimeZone(this DateTime dateTime, string timeZome)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZome));
        }
    }
}
