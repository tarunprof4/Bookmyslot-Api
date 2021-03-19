using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.Injections.Tests
{
    public class CustomerLastSharedSlotControllerTests
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
            var customerLastSharedSlotBusiness = serviceProvider.GetService<ICustomerLastSharedSlotBusiness>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();

            var controller = new CustomerLastSharedSlotController(customerLastSharedSlotBusiness, currentUser);

            Assert.IsNotNull(customerLastSharedSlotBusiness);
            Assert.IsNotNull(controller);
        }
    }
}
