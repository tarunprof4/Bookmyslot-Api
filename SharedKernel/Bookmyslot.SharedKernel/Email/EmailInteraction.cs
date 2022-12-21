using Bookmyslot.SharedKernel.Contracts.Email;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.SharedKernel.Email
{
    public class EmailInteraction : IEmailInteraction
    {
        private readonly IEmailClient emailClient;
        public EmailInteraction(IEmailClient emailClient)
        {
            this.emailClient = emailClient;
        }
        public async Task<Result<bool>> SendEmail(EmailModel emailModel)
        {
            var validateEmail = EmailValidator.Validate(emailModel);
            if (validateEmail.ResultType == ResultType.Success)
            {
                return await this.emailClient.SendEmail(emailModel);
            }

            return Result<bool>.ValidationError(validateEmail.Messages);
        }
    }
}
