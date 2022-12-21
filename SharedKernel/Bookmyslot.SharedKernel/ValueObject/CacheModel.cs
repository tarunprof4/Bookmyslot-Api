using System;

namespace Bookmyslot.SharedKernel.ValueObject
{
    public class CacheModel
    {
        public string Key { get; set; }
        public TimeSpan ExpiryTime { get; set; }

        public bool IsSlidingExpiry { get; set; }

        public static string GetSearchCustomerCacehKey(string searchKey, PageParameterModel pageParameterModel)
        {
            return string.Format("{0}-{1}-{2}", searchKey, pageParameterModel.PageNumber, pageParameterModel.PageSize);
        }
    }
}
