using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
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
            var customerBookedSlotBusiness = serviceProvider.GetService<ICustomerBookedSlotBusiness>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();
            var symmetryEncryption = serviceProvider.GetService<ISymmetryEncryption>();
            var customerResponseAdaptor = serviceProvider.GetService<ICustomerResponseAdaptor>();
            var cancelledSlotResponseAdaptor = serviceProvider.GetService<ICancelledSlotResponseAdaptor>();
            var bookedSlotResponseAdaptor = serviceProvider.GetService<IBookedSlotResponseAdaptor>();
            
            var controller = new CustomerBookedSlotController(customerBookedSlotBusiness, symmetryEncryption, currentUser,
                customerResponseAdaptor, cancelledSlotResponseAdaptor, bookedSlotResponseAdaptor);

            Assert.IsNotNull(customerBookedSlotBusiness);
            Assert.IsNotNull(symmetryEncryption);
            Assert.IsNotNull(controller);
        }
    }
}
