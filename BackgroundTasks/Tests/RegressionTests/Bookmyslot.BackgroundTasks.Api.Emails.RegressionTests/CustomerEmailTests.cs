//using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
//using Bookmyslot.Api.Common.Email;
//using Bookmyslot.Api.Common.Email.Configuration;
//using Bookmyslot.BackgroundTasks.Api.Contracts;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Threading.Tasks;


//namespace Bookmyslot.BackgroundTasks.Api.Emails.RegressionTests
//{
//    public class CustomerEmailTests
//    {
//        //////https://www.google.com/settings/security/lesssecureapps 
        
//        private readonly string SlotId = Guid.NewGuid().ToString();
//        private const string Title = "Title";
//        private const string CreatedBy = "CreatedBy";
//        private const string Country = "Country";
//        private const string MeetingLink = "https://meet.jit.si/ssdsdsdsdsd";
//        private readonly DateTime ValidSlotDate = DateTime.UtcNow.AddDays(2);
//        private readonly DateTime InValidSlotDate = DateTime.UtcNow.AddDays(-2);
//        private readonly TimeSpan ValidSlotStartTime = new TimeSpan(0, 0, 0);
//        private readonly TimeSpan ValidSlotEndTime = new TimeSpan(0, 30, 0);


//        private const string FirstName = "First";
//        private const string LastName = "Last";
//        private EmailClient emailClient;
//        private Mock<IHttpContextAccessor> httpContextAccessorMock;
//        private EmailConfiguration emailConfiguration;
//        private Mock<ILoggerService> loggerServiceMock;

//        [SetUp]
//        public void Setup()
//        {
//            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
//            emailConfiguration = new EmailConfiguration(configuration);
//            loggerServiceMock = new Mock<ILoggerService>();

//            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
//            emailClient = new EmailClient(httpContextAccessorMock.Object, emailConfiguration, loggerServiceMock.Object);
//        }

//        [Test]
//        public async Task RegisterCustomerWelcomeEmail()
//        {
//            var emailModel = CustomerEmailTemplateFactory.GetCustomerRegistrationWelcomeEmailTemplate(DefaultValidCreateCustomerModel());
//            var response = await emailClient.SendEmail(emailModel);
//            Assert.AreEqual(response.Result, true);
//        }


//        [Test]
//        public async Task SlotScheduledEmailTemplate()
//        {
//            var emailModel = CustomerEmailTemplateFactory.SlotBookedEmailTemplate(CreateValidSlotModel(), DefaultValidCreateCustomerModel());
//            var response = await emailClient.SendEmail(emailModel);

//            Assert.AreEqual(response.Result, true);
//        }


//        [Test]
//        public async Task SlotCancelledEmailTemplate()
//        {
//            var emailModel = CustomerEmailTemplateFactory.SlotCancelledEmailTemplate(CreateValidSlotModel(), DefaultValidCreateCustomerModel());
//            var response = await emailClient.SendEmail(emailModel);

//            Assert.AreEqual(response.Result, true);
//        }


//        [Test]
//        public async Task SlotMeetingInformationTemplate()
//        {
//            var emailModel = CustomerEmailTemplateFactory.SlotMeetingInformationTemplate(CreateValidSlotModel(), DefaultValidCreateCustomerModel());
//            var response = await emailClient.SendEmail(emailModel);

//            Assert.AreEqual(response.Result, true);
//        }




//        private SlotModel CreateValidSlotModel()
//        {
//            var slotModel = new SlotModel();
//            slotModel.Id = SlotId;
//            slotModel.Country = Country;
//            //slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(ValidSlotDate, TimeZoneConstants.IndianTimezone);
//            slotModel.Title = Title;
//            slotModel.CreatedBy = CreatedBy;
//            slotModel.SlotStartTime = ValidSlotStartTime;
//            slotModel.SlotEndTime = ValidSlotEndTime;

//            return slotModel;
//        }

//        private CustomerModel DefaultValidCreateCustomerModel()
//        {
//            var customerModel = new CustomerModel();
//            customerModel.FirstName = FirstName;
//            customerModel.LastName = LastName;
//            return customerModel;
//        }
//    }
//}