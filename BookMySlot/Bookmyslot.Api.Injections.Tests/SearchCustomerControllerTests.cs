using Bookmyslot.Api.Cache.Contracts.Configuration;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
namespace Bookmyslot.Api.Injections.Tests
{
    public class SearchCustomerControllerTests
    {
        private IServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            serviceProvider = webHost.Services;
            var configuration = serviceProvider.GetService<IConfiguration>();
            var Startup = new Startup(configuration);
        }

        [Test]
        public void StartupTest()
        {
            var searchCustomerBusiness = serviceProvider.GetService<ISearchCustomerBusiness>();
            var distributedInMemoryCacheBuisness = serviceProvider.GetService<IDistributedInMemoryCacheBuisness>();
            var cacheConfiguration = serviceProvider.GetService<CacheConfiguration>();

            var controller = new SearchCustomerController(searchCustomerBusiness, distributedInMemoryCacheBuisness, cacheConfiguration);

            Assert.IsNotNull(searchCustomerBusiness);
            Assert.IsNotNull(distributedInMemoryCacheBuisness);
            Assert.IsNotNull(controller);
        }
    }
}
