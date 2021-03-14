using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.Globalization;

namespace Bookmyslot.Api.Common.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool isDateValid(this string dateString)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString, DateTimeConstants.ApplicationInputDatePattern, CultureInfo.InvariantCulture,
          DateTimeStyles.None, out dateTime))
            {
                return true;
            }

            return false;
        }

    }
}

