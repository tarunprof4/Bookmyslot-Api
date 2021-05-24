﻿using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Encryption.Interfaces;
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
    public class CustomerSharedSlotControllerTests
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
            var customerSharedSlotBusiness = serviceProvider.GetService<ICustomerSharedSlotBusiness>();
            var symmetryEncryption = serviceProvider.GetService<ISymmetryEncryption>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();

            var controller = new CustomerSharedSlotController(customerSharedSlotBusiness, symmetryEncryption, currentUser);

            Assert.IsNotNull(customerSharedSlotBusiness);
            Assert.IsNotNull(symmetryEncryption);
            Assert.IsNotNull(controller);
        }
    }
}
