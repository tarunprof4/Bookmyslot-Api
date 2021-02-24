namespace Bookmyslot.Api.Cache.Contracts.Constants.cs
{
    public class CacheConstants
    {
        public const string GetDistinctCustomersNearestSlotFromTodayCacheKey = "GetDistinctCustomersNearestSlotFromToday-{0}";
        public const int GetDistinctCustomersNearestSlotFromTodayCacheExpiryMinutes = 60;

        public const string CustomerInfomationCacheKey = "GetCustomerInformation-{0}";
    }
}
