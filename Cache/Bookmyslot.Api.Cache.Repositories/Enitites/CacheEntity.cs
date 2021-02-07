using Dapper;
using System;

namespace Bookmyslot.Api.Cache.Repositories.Enitites
{
    [Table("Cache")]
    public class CacheEntity
    {
        public string CacheType { get; set; }
        public string CacheKey { get; set; }
        public string CacheValue { get; set; }
        public int ExpiryInSeconds { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
