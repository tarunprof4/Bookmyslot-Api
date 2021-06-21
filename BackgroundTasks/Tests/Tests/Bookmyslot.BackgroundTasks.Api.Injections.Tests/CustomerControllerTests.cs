using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
using Bookmyslot.BackgroundTasks.Api.Controllers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Bookmyslot.BackgroundTasks.Api.Injections.Tests
{



    public class CustomerControllerTests
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
            var customerBusiness = serviceProvider.GetService<ICustomerBusiness>();

            var controller = new CustomerController(customerBusiness);

            Assert.IsNotNull(customerBusiness);
            Assert.IsNotNull(controller);
        }
    }
}
