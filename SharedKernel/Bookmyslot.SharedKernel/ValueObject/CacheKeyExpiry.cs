using System;
using System.Collections.Generic;

namespace Bookmyslot.SharedKernel.ValueObject
{
    public class CacheKeyExpiry : BaseValueObject
    {
        public string Key { get; set; }
        public TimeSpan ExpiryTime { get; set; }

        public bool IsSlidingExpiry { get; set; }

        public static string GetSearchCustomerCacehKey(string searchKey, PageParameter pageParameterModel)
        {
            return string.Format("{0}-{1}-{2}", searchKey, pageParameterModel.PageNumber, pageParameterModel.PageSize);
        }
    }
}
