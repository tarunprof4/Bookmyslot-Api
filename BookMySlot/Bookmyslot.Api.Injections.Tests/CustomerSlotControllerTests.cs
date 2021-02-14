using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
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
            var keyEncryptor = serviceProvider.GetService<IKeyEncryptor>();
            var distributedInMemoryCacheBuisness = serviceProvider.GetService<IDistributedInMemoryCacheBuisness>();
            var hash = serviceProvider.GetService<IHashing>();
            var controller = new CustomerSlotController(customerSlotBusiness, keyEncryptor, distributedInMemoryCacheBuisness, hash);

            Assert.IsNotNull(customerSlotBusiness);
            Assert.IsNotNull(keyEncryptor);
            Assert.IsNotNull(controller);
        }
    }
}
