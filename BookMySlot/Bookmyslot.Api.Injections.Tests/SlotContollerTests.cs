using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Location.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.Injections.Tests
{
    public class SlotControllerTests
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
            var slotBusiness = serviceProvider.GetService<ISlotBusiness>();
            var keyEncryptor = serviceProvider.GetService<IKeyEncryptor>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();
            var nodaTimeZoneLocationBusiness = serviceProvider.GetService<INodaTimeZoneLocationBusiness>();

            var controller = new SlotController(slotBusiness, keyEncryptor, currentUser, nodaTimeZoneLocationBusiness);

            Assert.IsNotNull(slotBusiness);
            Assert.IsNotNull(keyEncryptor);
            Assert.IsNotNull(controller);
        }
    }
}