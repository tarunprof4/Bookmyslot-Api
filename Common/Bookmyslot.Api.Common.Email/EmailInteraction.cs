using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Email.Interfaces;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Email
{
    public class EmailInteraction : IEmailInteraction
    {
        private readonly IEmailClient emailClient;
        public EmailInteraction(IEmailClient emailClient)
        {
            this.emailClient = emailClient;
        }
        public async Task<Response<bool>> SendEmail(EmailModel emailModel)
        {
            var validateEmail = EmailValidator.Validate(emailModel);
            if(validateEmail.ResultType == ResultType.Success)
            {
                return await this.emailClient.SendEmail(emailModel);
            }

            return Response<bool>.ValidationError(validateEmail.Messages);
        }
    }
}
