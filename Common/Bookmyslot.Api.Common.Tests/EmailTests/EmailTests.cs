using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Email;
using Bookmyslot.Api.Common.Email.Constants;
using NUnit.Framework;
using System.Collections.Generic;

namespace Bookmyslot.Api.Common.Tests.EmailTests
{
    public class EmailTests
    {
        private const string From = "from@gmail.com";
        private const string InvalidFrom = "InvalidFrom";
        private const string To = "to@gmail.com";
        private const string InvalidTo = "InvalidTo";
        private const string cc = "cc@gmail.com";
        private const string InvalidCC = "InvalidCC";
        private const string bcc = "bcc@gmail.com";
        private const string InvalidBcc = "InvalidBcc";
        private const string Subject = "subject";
        private const string Body = "body";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EmailValidator_ValidEmail_ReturnsSuccessResponse()
        {
            EmailModel emailModel = GetDefaultValidEmailModel();
            var validateEmailResponse = EmailValidator.Validate(emailModel);

            Assert.AreEqual(validateEmailResponse.ResultType, ResultType.Success);
            Assert.AreEqual(validateEmailResponse.Result, true);
        }

        [Test]
        public void EmailValidator_EmptyEmailModel_ReturnsValidationErrorResponse()
        {
            EmailModel emailModel = GetDefaultEmptyEmailModel();
            var validateEmailResponse = EmailValidator.Validate(emailModel);

            Assert.AreEqual(validateEmailResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(validateEmailResponse.Result, false);
            Assert.IsTrue(validateEmailResponse.Messages.Contains(EmailConstants.EmailSubjectIsRequired));
            Assert.IsTrue(validateEmailResponse.Messages.Contains(EmailConstants.EmailBodyIsRequired));
            Assert.IsTrue(validateEmailResponse.Messages.Contains(EmailConstants.EmailToAddressIsRequired));
        }


        [Test]
        public void EmailValidator_InValidEmailModel_ReturnsValidationErrorResponse()
        {
            EmailModel emailModel = GetDefaultInvalidEmailModel();
            var validateEmailResponse = EmailValidator.Validate(emailModel);

            Assert.AreEqual(validateEmailResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(validateEmailResponse.Result, false);
            Assert.IsTrue(validateEmailResponse.Messages.Contains(string.Format(EmailConstants.EmailToAddressIsInvalid, InvalidTo)));
            Assert.IsTrue(validateEmailResponse.Messages.Contains(string.Format(EmailConstants.EmailccAddressIsInvalid, InvalidCC)));
            Assert.IsTrue(validateEmailResponse.Messages.Contains(string.Format(EmailConstants.EmailBccAddressIsInvalid, InvalidBcc)));
        }
        private EmailModel GetDefaultValidEmailModel()
        {
            var emailModel = new EmailModel();
            emailModel.To = new List<string>() { To };
            emailModel.From = From;
            emailModel.Cc = new List<string>() { cc };
            emailModel.Bcc = new List<string>() { bcc };
            emailModel.Subject = Subject;
            emailModel.Body = Body;

            return emailModel;
        }

        private EmailModel GetDefaultEmptyEmailModel()
        {
            var emailModel = new EmailModel();
            emailModel.To = new List<string>() {  };
            emailModel.Cc = new List<string>() {  };
            emailModel.Bcc = new List<string>() {  };

            return emailModel;
        }

        private EmailModel GetDefaultInvalidEmailModel()
        {
            var emailModel = new EmailModel();
            emailModel.From = InvalidFrom;
            emailModel.To = new List<string>() { InvalidTo };
            emailModel.Cc = new List<string>() { InvalidCC };
            emailModel.Bcc = new List<string>() { InvalidBcc };

            return emailModel;
        }
    }
}