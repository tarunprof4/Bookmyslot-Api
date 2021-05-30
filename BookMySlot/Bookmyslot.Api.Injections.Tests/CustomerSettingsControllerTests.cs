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
    public class CustomerSettingsControllerTests
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
            var customerSettingsBusiness = serviceProvider.GetService<ICustomerSettingsBusiness>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();
            var customerSettingsViewModelValidator = serviceProvider.GetService<IValidator<CustomerSettingsViewModel>>();

            var controller = new CustomerSettingsController(customerSettingsBusiness, currentUser, customerSettingsViewModelValidator);

            Assert.IsNotNull(customerSettingsBusiness);
            Assert.IsNotNull(controller);
        }
    }
}
