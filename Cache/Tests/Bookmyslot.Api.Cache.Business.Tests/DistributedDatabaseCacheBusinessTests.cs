using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Business.Tests
{
    public class DistributedDatabaseCaceBusinessTests
    {
        private const string KEY = "key";
        private IDistributedDatabaseCacheBuisness distributedDatabaseCacheBuisness;
        private Mock<IDistributedCache> distributedCacheMock;



        [SetUp]
        public void Setup()
        {
            distributedCacheMock = new Mock<IDistributedCache>();
            distributedDatabaseCacheBuisness = new DistributedDatabaseCacheBusiness(distributedCacheMock.Object);
        }


        [Test]
        public async Task GetFromCacheAsync_CacheReturnsResponse_FunctionIsNotInvoked()
        {
            var cacheModel = GetDefaultCacheModel();
            var keyStringBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(KEY));
            distributedCacheMock.Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(keyStringBytes));

            var cacheResponse = await distributedDatabaseCacheBuisness.GetFromCacheAsync(cacheModel, () => Invoke(KEY));

            distributedCacheMock.Verify((m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())), Times.Once());
            distributedCacheMock.Verify((m => m.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())), Times.Never());
        }
        


        [Test]
        public async Task GetFromCacheAsync_CacheReturnsNoResponse_FunctionIsInvoked()
        {
            var cacheModel = GetDefaultCacheModel();
            byte[] emptyBytes = null;
            var keyStringBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(KEY));
            distributedCacheMock.Setup(a => a.GetAsync(It.IsAny<string>(), new CancellationToken())).Returns(Task.FromResult(emptyBytes));
            distributedCacheMock.Setup(m => m.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(keyStringBytes));

            var cacheResponse = await distributedDatabaseCacheBuisness.GetFromCacheAsync(cacheModel, () => Invoke(KEY));

            distributedCacheMock.Verify((m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())), Times.Once());
            distributedCacheMock.Verify((m => m.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())), Times.Once());
        }




        private Task<Response<string>> Invoke(string name)
        {
            var response = new Response<string>() { Result = name };
            return Task.FromResult(response);
        }

        private CacheModel GetDefaultCacheModel()
        {
            var cacheModel = new CacheModel();
            cacheModel.Key = KEY;
            cacheModel.ExpiryTimeUtc = new TimeSpan(0, 0, 20);
            cacheModel.IsSlidingExpiry = false;
            return cacheModel;
        }
    }
}