//using Bookmyslot.Api.Cache.Contracts;
//using Bookmyslot.Api.Cache.Contracts.Interfaces;
//using Bookmyslot.Api.Common.Compression.Interfaces;
//using Bookmyslot.Api.Common.Contracts;
//using Microsoft.Extensions.Caching.Distributed;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Bookmyslot.Api.Cache.Business.Tests
//{
//    public class DatabaseCacheBusinessTests
//    {
//        private const string KEY = "key";
//        private IDistributedDatabaseCacheBuisness distributedDatabaseCacheBuisness;
//        private Mock<IDistributedCache> distributedCacheMock;
//        private Mock<ICompression> compressionMock;



//        [SetUp]
//        public void Setup()
//        {
//            distributedCacheMock = new Mock<IDistributedCache>();
//            compressionMock = new Mock<ICompression>();
//            distributedDatabaseCacheBuisness = new DistributedDatabaseCacheBuisness(distributedCacheMock.Object, compressionMock.Object);
//        }


//        [Test]
//        public async Task GetFromCacheAsync_CacheReturnsResponse_FunctionIsNotInvoked()
//        {
//            var cacheModel = GetDefaultCacheModel();
//            distributedCacheMock.Setup(a => a.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(KEY));

//            var cacheResponse = await distributedDatabaseCacheBuisness.GetFromCacheAsync(cacheModel, () => Invoke(KEY));

//            distributedCacheMock.Verify((m => m.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())), Times.Once());
//            distributedCacheMock.Verify((m => m.SetStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())), Times.Never());
//            compressionMock.Verify((m => m.Compress(It.IsAny<string>())), Times.Never());
//            compressionMock.Verify((m => m.Decompress<object>(It.IsAny<string>())), Times.Once());
//        }


//        [Test]
//        public async Task GetFromCacheAsync_CacheReturnsNoResponse_FunctionIsInvoked()
//        {
//            var cacheModel = GetDefaultCacheModel();
//            distributedCacheMock.Setup(a => a.GetStringAsync(It.IsAny<string>(), new CancellationToken())).Returns(Task.FromResult(string.Empty));
//            Response<bool> createCacheResponse = new Response<bool>() { Result = true };
//            distributedCacheMock.Setup(m => m.SetStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(""));

//            var cacheResponse = await distributedDatabaseCacheBuisness.GetFromCacheAsync(cacheModel, () => Invoke(KEY));

//            distributedCacheMock.Verify((m => m.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())), Times.Once());
//            distributedCacheMock.Verify((m => m.SetStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())), Times.Once());
//            compressionMock.Verify((m => m.Compress(It.IsAny<string>())), Times.Once());
//            compressionMock.Verify((m => m.Decompress<object>(It.IsAny<string>())), Times.Never());
//        }




//        private Task<Response<string>> Invoke(string name)
//        {
//            var response = new Response<string>() { Result = name };
//            return Task.FromResult(response);
//        }

//        private CacheModel GetDefaultCacheModel()
//        {
//            var cacheModel = new CacheModel();
//            cacheModel.Key = KEY;
//            cacheModel.ExpiryTimeUtc = new TimeSpan(0, 0, 20);
//            cacheModel.IsSlidingExpiry = false;
//            return cacheModel;
//        }
//    }
//}