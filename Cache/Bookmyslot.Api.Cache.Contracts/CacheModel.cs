using System;

namespace Bookmyslot.Api.Cache.Contracts
{
    public class CacheModel
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public TimeSpan ExpiryTimeUtc { get; set; }
    }
}
