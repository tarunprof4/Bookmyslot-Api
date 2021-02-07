using Bookmyslot.Api.Cache.Repositories.Enitites;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using System.Collections.Generic;

namespace Bookmyslot.Api.Cache.Repositories.ModelFactory
{
    internal class ResponseModelFactory
    {

        internal static Response<string> CreateCacheValueResponse(CacheEntity cacheEntity)
        {
            if (cacheEntity == null)
            {
                return Response<string>.Empty(new List<string>() { AppBusinessMessagesConstants.EmptyCache });
            }

            var cacheValue = ModelFactory.CreateCacheValue(cacheEntity);
            return new Response<string>() { Result = cacheValue };
        }

    }
}
