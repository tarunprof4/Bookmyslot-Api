using System;

namespace Bookmyslot.Api.Cache.Contracts
{
    public class CacheModel
    {
        public string Key { get; set; }
        public TimeSpan ExpiryTime { get; set; }

        public bool IsSlidingExpiry { get; set; }
    }
}
