using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Business.Tests
{
    public class DatabaseCacheBusinessTests
    {
        private const string KEY = "key";
        private IDatabaseCacheBuisness sqlTableCacheHandler;
        private Mock<ICacheRepository> cacheRepositoryMock;
        private Mock<ICompression> compressionMock;



        [SetUp]
        public void Setup()
        {
            cacheRepositoryMock = new Mock<ICacheRepository>();
            compressionMock = new Mock<ICompression>();
            sqlTableCacheHandler = new DatabaseCacheBusiness(cacheRepositoryMock.Object, compressionMock.Object);
        }


        [Test]
        public async Task GetFromCacheAsync_CacheReturnsResponse_FunctionIsNotInvoked()
        {
            Response<string> cacheResponse = new Response<string>() { Result = KEY };
            cacheRepositoryMock.Setup(a => a.GetCache(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(cacheResponse));

            var sqlTableCacheHandlerResponse = await sqlTableCacheHandler.GetFromCacheAsync(KEY, () => Invoke(KEY), 60);

            cacheRepositoryMock.Verify((m => m.GetCache(It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            cacheRepositoryMock.Verify((m => m.CreateCache(It.IsAny<CacheModel>())), Times.Never());
            compressionMock.Verify((m => m.Compress(It.IsAny<string>())), Times.Never());
            compressionMock.Verify((m => m.Decompress<object>(It.IsAny<string>())), Times.Once());
        }


        [Test]
        public async Task GetFromCacheAsync_CacheReturnsNoResponse_FunctionIsInvoked()
        {
            Response<string> cacheResponse = new Response<string>() { ResultType = ResultType.Empty };
            cacheRepositoryMock.Setup(a => a.GetCache(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(cacheResponse));
            Response<bool> createCacheResponse = new Response<bool>() { Result = true };
            cacheRepositoryMock.Setup(a => a.CreateCache(It.IsAny<CacheModel>())).Returns(Task.FromResult(createCacheResponse));

            var sqlTableCacheHandlerResponse = await sqlTableCacheHandler.GetFromCacheAsync(KEY, () => Invoke(KEY), 60);

            cacheRepositoryMock.Verify((m => m.GetCache(It.IsAny<string>(), It.IsAny<string>())), Times.Once());
            cacheRepositoryMock.Verify((m => m.CreateCache(It.IsAny<CacheModel>())), Times.Once());
            compressionMock.Verify((m => m.Compress(It.IsAny<string>())), Times.Once());
            compressionMock.Verify((m => m.Decompress<object>(It.IsAny<string>())), Times.Never());
        }

     


        private Task<Response<string>> Invoke(string name)
        {
            var response = new Response<string>() { Result = name };
            return Task.FromResult(response);
        }
    }
}