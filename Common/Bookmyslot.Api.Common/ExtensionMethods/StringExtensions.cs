using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.Globalization;

namespace Bookmyslot.Api.Common.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool isDateValid(this string dateString, string datePattern)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString, datePattern,  CultureInfo.InvariantCulture,
          DateTimeStyles.None, out dateTime))
            {
                return true;
            }

            return false;
        }

    }
}

