using Bookmyslot.SharedKernel.Contracts.Email;
using Bookmyslot.SharedKernel.Contracts.Logging;
using Bookmyslot.SharedKernel.Email.Configuration;
using Bookmyslot.SharedKernel.Email.Constants;
using Bookmyslot.SharedKernel.Logging.Constants;
using Bookmyslot.SharedKernel.Logging.Contracts;
using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.SharedKernel.Email
{
    public class EmailClient : IEmailClient
    {
        private readonly SmtpClient smtpClient;
        private readonly string fromEmailAddress;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILoggerService loggerService;

        public EmailClient(IHttpContextAccessor httpContextAccessor, EmailConfiguration emailConfiguration, ILoggerService loggerService)
        {
            this.smtpClient = new SmtpClient();
            this.smtpClient.UseDefaultCredentials = false;
            this.smtpClient.EnableSsl = true;
            this.smtpClient.Port = Convert.ToInt32(emailConfiguration.EmailPort);
            this.smtpClient.Host = emailConfiguration.EmailStmpHost;
            this.smtpClient.Credentials = new NetworkCredential(emailConfiguration.EmailUserName, emailConfiguration.EmailPassword);
            this.smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            this.fromEmailAddress = emailConfiguration.EmailUserName;

            this.httpContextAccessor = httpContextAccessor;
            this.loggerService = loggerService;
        }
        public async Task<Result<bool>> SendEmail(EmailModel emailModel)
        {
            try
            {
                var mailMessage = CreateMailMessage(emailModel);
                await this.smtpClient.SendMailAsync(mailMessage);
                return Result<bool>.Success(true);
            }
            catch (Exception exp)
            {
                var coorelationId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.CoorelationId];
                var emaillog = new EmailLog(coorelationId);
                this.loggerService.Error(exp, "{@emaillog}", emaillog);
                return Result<bool>.Error(new List<string>() { EmailConstants.SendEmailFailure });
            }
        }
        private MailMessage CreateMailMessage(EmailModel emailModel)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(this.fromEmailAddress);
            foreach (var recipient in emailModel.To)
            {
                mailMessage.To.Add(recipient);
            }

            if (emailModel.Cc != null)
            {
                foreach (var recipient in emailModel.Cc)
                {
                    mailMessage.CC.Add(recipient);
                }
            }

            if (emailModel.Bcc != null)
            {
                foreach (var recipient in emailModel.Bcc)
                {
                    mailMessage.Bcc.Add(recipient);
                }
            }

            mailMessage.IsBodyHtml = emailModel.IsBodyHtml;
            mailMessage.Subject = emailModel.Subject;
            mailMessage.Body = emailModel.Body;

            if (!string.IsNullOrEmpty(emailModel.HtmlAttachment))
            {
                var byteArray = Encoding.GetEncoding("UTF-8").GetBytes(emailModel.HtmlAttachment);
                var memoryStream = new System.IO.MemoryStream(byteArray);
                var attachment = new Attachment(memoryStream, emailModel.FileName);
                mailMessage.Attachments.Add(attachment);
            }

            return mailMessage;
        }
    }
}
