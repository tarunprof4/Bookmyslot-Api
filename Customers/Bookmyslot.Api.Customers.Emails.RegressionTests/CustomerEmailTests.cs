using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Email;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Emails.RegressionTests
{
    public class CustomerEmailTests
    {
        private readonly string SlotId = Guid.NewGuid().ToString();
        private const string Title = "Title";
        private const string CreatedBy = "CreatedBy";
        private const string Country = "Country";
        private const string deletedBy = "deletedBy";
        private readonly DateTime ValidSlotDate = DateTime.UtcNow.AddDays(2);
        private readonly DateTime InValidSlotDate = DateTime.UtcNow.AddDays(-2);
        private readonly TimeSpan ValidSlotStartTime = new TimeSpan(0, 0, 0);
        private readonly TimeSpan ValidSlotEndTime = new TimeSpan(0, SlotConstants.MinimumSlotDuration, 0);


        private const string FirstName = "First";
        private const string LastName = "Last";
        private EmailClient emailClient;
        private Mock<IHttpContextAccessor> httpContextAccessorMock;
        private EmailConfiguration emailConfiguration;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            emailConfiguration = new EmailConfiguration(configuration);

            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            emailClient = new EmailClient(httpContextAccessorMock.Object, emailConfiguration);
        }

        [Test]
        public async Task RegisterCustomerWelcomeEmail()
        {
            var emailModel = CustomerEmailTemplateFactory.GetCustomerRegistrationWelcomeEmailTemplate(DefaultValidCreateCustomerModel());
            var response = await emailClient.SendEmail(emailModel);
            Assert.AreEqual(response.Result, true);
        }


        [Test]
        public async Task SlotScheduledEmailTemplate()
        {
            var emailModel = CustomerEmailTemplateFactory.SlotScheduledEmailTemplate(CreateValidSlotModel(), DefaultValidCreateCustomerModel());
            var response = await emailClient.SendEmail(emailModel);

            Assert.AreEqual(response.Result, true);
        }


        [Test]
        public async Task SlotCancelledEmailTemplate()
        {
            var emailModel = CustomerEmailTemplateFactory.SlotCancelledEmailTemplate(CreateValidSlotModel(), DefaultValidCreateCustomerModel());
            var response = await emailClient.SendEmail(emailModel);

            Assert.AreEqual(response.Result, true);
        }




        private SlotModel CreateValidSlotModel()
        {
            var slotModel = new SlotModel();
            slotModel.Id = SlotId;
            slotModel.Country = Country;
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);
            slotModel.Title = Title;
            slotModel.CreatedBy = CreatedBy;
            slotModel.SlotStartTime = ValidSlotStartTime;
            slotModel.SlotEndTime = ValidSlotEndTime;

            return slotModel;
        }

        private CustomerModel DefaultValidCreateCustomerModel()
        {
            var customerModel = new CustomerModel();
            customerModel.FirstName = FirstName;
            customerModel.LastName = LastName;
            return customerModel;
        }
    }
}