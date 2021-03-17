using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts.Configuration;
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
    public class CustomerBookedSlotControllerTests
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

            var env = serviceProvider.GetService<AppConfiguration>();
            var customerBookedSlotBusiness = serviceProvider.GetService<ICustomerBookedSlotBusiness>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();
            var keyEncryptor = serviceProvider.GetService<IKeyEncryptor>();

            var controller = new CustomerBookedSlotController(customerBookedSlotBusiness, keyEncryptor, currentUser);

            Assert.IsNotNull(customerBookedSlotBusiness);
            Assert.IsNotNull(keyEncryptor);
            Assert.IsNotNull(controller);
        }
    }
}
