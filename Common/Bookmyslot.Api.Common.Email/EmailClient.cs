using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Net;
using Serilog;
using Bookmyslot.Api.Common.Contracts.Constants;

namespace Bookmyslot.Api.Common.Email
{
    public class EmailClient : IEmailClient
    {
        private readonly SmtpClient smtpClient;

        public EmailClient(IConfiguration configuration)
        {
            this.smtpClient = new SmtpClient(configuration.GetSection("SmtpHost").Value);
            this.smtpClient.Port = 587;
            this.smtpClient.Credentials = new NetworkCredential(
                configuration.GetSection("User").Value,
                configuration.GetSection("Password").Value);
            this.smtpClient.EnableSsl = true;
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
                Log.Information(EmailConstants.SendEmailFailure + exp);
                return Response<bool>.Failed(new List<string>() { EmailConstants.SendEmailFailure });
            }
        }

        private MailMessage CreateMailMessage(EmailModel emailModel)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailModel.From);
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
