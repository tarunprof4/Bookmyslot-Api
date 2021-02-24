using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Constants.cs;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Business.Tests
{
    public class CacheTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckIfAllCacheKeysAreUnique()
        {
            var cacheKeys = new Dictionary<string, string>();
            
            cacheKeys.Add(CacheConstants.GetDistinctCustomersNearestSlotFromTodayCacheKey, CacheConstants.GetDistinctCustomersNearestSlotFromTodayCacheKey);
            cacheKeys.Add(CacheConstants.CustomerInfomationCacheKey, CacheConstants.CustomerInfomationCacheKey);
        }


    }
}