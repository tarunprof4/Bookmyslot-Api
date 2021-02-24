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
using System.Linq;
using System.Reflection;
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
            var cacheKeyDictionary = new Dictionary<string, string>();
            List<string> cacheKeys = typeof(CacheConstants).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();

            foreach (var cacheKey in cacheKeys)
            {
                cacheKeyDictionary.Add(cacheKey, cacheKey);
            }
        }
    }
}