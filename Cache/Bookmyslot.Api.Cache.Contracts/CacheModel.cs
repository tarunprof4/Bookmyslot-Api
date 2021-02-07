namespace Bookmyslot.Api.Cache.Contracts
{
    public class CacheModel
    {
        public string Type;
        public string Key;
        public string Value;
        public int ExpiryInSeconds;
    }
}
