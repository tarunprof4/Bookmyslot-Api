//using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
//using Microsoft.AspNetCore;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using NUnit.Framework;
//using System;

//namespace Bookmyslot.Api.SlotScheduler.Injections.Tests
//{
//    public class SlotControllerTests
//    {
//        private IServiceProvider serviceProvider;

//        [SetUp]
//        public void Setup()
//        {
//            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
//            serviceProvider = webHost.Services;
//            var configuration = serviceProvider.GetService<IConfiguration>();
//            var Startup = new Startup(configuration);
//        }

//        [Test]
//        public void StartupTest()
//        {
//            var slotBusiness = serviceProvider.GetService<ISlotBusiness>();
//            var controller = new SlotController(slotBusiness);

//            Assert.IsNotNull(slotBusiness);
//            Assert.IsNotNull(controller);
//        }
//    }
//}