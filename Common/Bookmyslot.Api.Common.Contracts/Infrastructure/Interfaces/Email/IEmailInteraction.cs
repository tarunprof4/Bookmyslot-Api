using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email
{
    public interface IEmailInteraction
    {
        Task<Response<bool>> SendEmail(EmailModel emailModel);
    }
}
