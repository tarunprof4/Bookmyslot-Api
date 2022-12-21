using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.SharedKernel.ValueObject;
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
    public class DistributedInMemoryCacheBusinessTests
    {
        private const string KEY = "key";
        private IDistributedInMemoryCacheBuisness distributedInMemoryCacheBuisness;
        private Mock<IDistributedCache> distributedCacheMock;



        [SetUp]
        public void Setup()
        {
            distributedCacheMock = new Mock<IDistributedCache>();
            distributedInMemoryCacheBuisness = new DistributedInMemoryCacheBusiness(distributedCacheMock.Object);
        }


        [Test]
        public async Task GetFromCacheAsync_CacheReturnsResponse_FunctionIsNotInvoked()
        {
            var cacheModel = GetDefaultCacheModel();
            var keyStringBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(KEY));
            distributedCacheMock.Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(keyStringBytes));

            var cacheResponse = await distributedInMemoryCacheBuisness.GetFromCacheAsync(cacheModel, () => Invoke(KEY));

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

            var cacheResponse = await distributedInMemoryCacheBuisness.GetFromCacheAsync(cacheModel, () => Invoke(KEY));

            distributedCacheMock.Verify((m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())), Times.Once());
            distributedCacheMock.Verify((m => m.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())), Times.Once());
        }




        private Task<Result<string>> Invoke(string name)
        {
            var response = new Result<string>() { Value = name };
            return Task.FromResult(response);
        }

        private CacheModel GetDefaultCacheModel()
        {
            var cacheModel = new CacheModel();
            cacheModel.Key = KEY;
            cacheModel.ExpiryTime = TimeSpan.FromSeconds(20);
            cacheModel.IsSlidingExpiry = false;
            return cacheModel;
        }
    }
}