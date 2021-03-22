using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.Injections.Tests
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
            var currentUser = serviceProvider.GetService<ICurrentUser>();

            var controller = new CustomerController(customerBusiness, currentUser);

            Assert.IsNotNull(customerBusiness);
            Assert.IsNotNull(controller);
        }
    }
}
