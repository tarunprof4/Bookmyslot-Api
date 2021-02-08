using System;

namespace Bookmyslot.Api.Cache.Contracts
{
    public class CacheModel
    {
        public string Key { get; set; }
        public TimeSpan ExpiryTimeUtc { get; set; }

        public bool IsSlidingExpiry { get; set; }
    }
}
