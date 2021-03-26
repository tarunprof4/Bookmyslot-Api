using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.File.Contracts.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
namespace Bookmyslot.Api.Injections.Tests
{

    public class ProfilePictureControllerTests
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
            var profileSettingsBusiness = serviceProvider.GetService<IProfileSettingsBusiness>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();
            var fileConfigurationBusiness = serviceProvider.GetService<IFileConfigurationBusiness>();

            var controller = new ProfilePictureController(profileSettingsBusiness, currentUser, fileConfigurationBusiness);

            Assert.IsNotNull(profileSettingsBusiness);
            Assert.IsNotNull(controller);
        }
    }
}
