using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Encryption.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.NodaTime.Interfaces;
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
            var symmetryEncryption = serviceProvider.GetService<ISymmetryEncryption>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();
            var nodaTimeZoneLocationBusiness = serviceProvider.GetService<INodaTimeZoneLocationBusiness>();

            var controller = new SlotController(slotBusiness, symmetryEncryption, currentUser, nodaTimeZoneLocationBusiness);

            Assert.IsNotNull(slotBusiness);
            Assert.IsNotNull(symmetryEncryption);
            Assert.IsNotNull(controller);
        }
    }
}