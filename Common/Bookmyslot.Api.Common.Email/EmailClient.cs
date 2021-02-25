using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Email.Constants;
using Bookmyslot.Api.Common.Email.Interfaces;
using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Email
{
    public class EmailClient : IEmailClient
    {
        private readonly SmtpClient smtpClient;
        private readonly string fromEmailAddress;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration emailConfiguration;

        public EmailClient(IHttpContextAccessor httpContextAccessor, EmailConfiguration emailConfiguration)
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
        }


        public async Task<Response<bool>> SendEmail(EmailModel emailModel)
        {
            try
            {
                var mailMessage = CreateMailMessage(emailModel);
                await this.smtpClient.SendMailAsync(mailMessage);
                return Response<bool>.Success(true);
            }
            catch (Exception exp)
            {
                var requestId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.RequestId];
                var emaillog = new EmailLog(requestId);
                Log.Error(exp, "{@emaillog}", emaillog);
                return Response<bool>.Error(new List<string>() { EmailConstants.SendEmailFailure });
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
