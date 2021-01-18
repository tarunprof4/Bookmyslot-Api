using Bookmyslot.Api.Common.Contracts.Constants;
using System;

namespace Bookmyslot.Api.Common.Contracts.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime ConvertDateStringToDate(string dateString)
        {
            var inputDateString = dateString.Split(DateTimeConstants.DateDelimiter);
            var date = Convert.ToInt32(inputDateString[0]);
            var month = Convert.ToInt32(inputDateString[1]);
            var year = Convert.ToInt32(inputDateString[2]);

            return new DateTime(year, month, date, 0, 0, 0, DateTimeKind.Unspecified);
            
        }
    }
}
