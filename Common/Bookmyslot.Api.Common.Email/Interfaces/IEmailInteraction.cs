using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Email.Interfaces
{
    public interface IEmailInteraction
    {
        Task<Response<bool>> SendEmail(EmailModel emailModel);
    }
}
