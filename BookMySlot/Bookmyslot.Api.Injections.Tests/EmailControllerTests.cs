using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Controllers;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using FluentValidation;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Bookmyslot.Api.Injections.Tests
{
    public class EmailControllerTests
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
            var symmetryEncryption = serviceProvider.GetService<ISymmetryEncryption>();
            var resendSlotInformationBusiness = serviceProvider.GetService<IResendSlotInformationBusiness>();
            var currentUser = serviceProvider.GetService<ICurrentUser>();
            var resendSlotInformationViewModelValidator = serviceProvider.GetService<IValidator<ResendSlotInformationViewModel>>();

            var controller = new EmailController(symmetryEncryption, resendSlotInformationBusiness, currentUser, resendSlotInformationViewModelValidator);

            Assert.IsNotNull(symmetryEncryption);
            Assert.IsNotNull(resendSlotInformationBusiness);
            Assert.IsNotNull(controller);
        }
    }
}