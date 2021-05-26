using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
namespace Bookmyslot.Api.Injections.Tests
{
    public class CustomerSlotControllerTests
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
            var customerSlotBusiness = serviceProvider.GetService<ICustomerSlotBusiness>();
            var symmetryEncryption = serviceProvider.GetService<ISymmetryEncryption>();
            var distributedInMemoryCacheBuisness = serviceProvider.GetService<IDistributedInMemoryCacheBuisness>();
            var hash = serviceProvider.GetService<IHashing>();
            var cacheConfiguration = serviceProvider.GetService<CacheConfiguration>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();


            var controller = new CustomerSlotController(customerSlotBusiness, symmetryEncryption, distributedInMemoryCacheBuisness, hash, cacheConfiguration, currentUser);

            Assert.IsNotNull(customerSlotBusiness);
            Assert.IsNotNull(symmetryEncryption);
            Assert.IsNotNull(controller);
        }
    }
}
