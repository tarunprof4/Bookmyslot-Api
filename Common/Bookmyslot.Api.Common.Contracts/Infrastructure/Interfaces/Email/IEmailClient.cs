using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email
{
    public interface IEmailClient
    {
        Task<Response<bool>> SendEmail(EmailModel emailModel);
    }
}
