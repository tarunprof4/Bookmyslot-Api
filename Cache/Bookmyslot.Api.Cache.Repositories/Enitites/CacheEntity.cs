using Bookmyslot.Api.Common.Database.Constants;
using Dapper;
using System;

namespace Bookmyslot.Api.Cache.Repositories.Enitites
{
    [Table(TableNameConstants.Cache)]
    public class CacheEntity
    {
        public string CacheType { get; set; }
        public string CacheKey { get; set; }
        public string CacheValue { get; set; }
        public DateTime ExpiryTimeUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }

    }
}
