using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
namespace Bookmyslot.Api.Injections.Tests
{

    public class AdditionalProfileSettingsControllerTests
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
            var additionalProfileSettingsBusiness = serviceProvider.GetService<IAdditionalProfileSettingsBusiness>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();
            var additionalProfileSettingsViewModelValidator = serviceProvider.GetService<IValidator<AdditionalProfileSettingsViewModel>>();

            var controller = new AdditionalProfileSettingsController(additionalProfileSettingsBusiness, currentUser,
                additionalProfileSettingsViewModelValidator);

            Assert.IsNotNull(additionalProfileSettingsBusiness);
            Assert.IsNotNull(controller);
        }
    }
}
