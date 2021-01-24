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
    public class EmailContollerTests
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
            var keyEncryptor = serviceProvider.GetService<IKeyEncryptor>();
            var resendSlotInformationBusiness = serviceProvider.GetService<IResendSlotInformationBusiness>();

            var controller = new EmailController(keyEncryptor, resendSlotInformationBusiness);

            Assert.IsNotNull(keyEncryptor);
            Assert.IsNotNull(resendSlotInformationBusiness);
            Assert.IsNotNull(controller);
        }
    }
}