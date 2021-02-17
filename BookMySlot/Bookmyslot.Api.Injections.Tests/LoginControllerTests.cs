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
    public class LoginControllerTests
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
            var loginCustomerBusiness = serviceProvider.GetService<ILoginCustomerBusiness>();

            var controller = new LoginController(loginCustomerBusiness);

            Assert.IsNotNull(loginCustomerBusiness);
            Assert.IsNotNull(controller);
        }
    }
}