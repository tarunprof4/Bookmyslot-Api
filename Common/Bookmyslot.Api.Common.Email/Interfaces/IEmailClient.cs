using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Email.Interfaces
{
    public interface IEmailClient
    {
        Task<Response<bool>> SendEmail(EmailModel emailModel);
    }
}
