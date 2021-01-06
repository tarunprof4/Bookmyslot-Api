using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Controllers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
namespace Bookmyslot.Api.SlotScheduler.Injections.Tests
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
            var controller = new CustomerSlotController(customerSlotBusiness);

            Assert.IsNotNull(customerSlotBusiness);
            Assert.IsNotNull(controller);
        }
    }
}
