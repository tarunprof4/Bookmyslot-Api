using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Contracts.Interfaces
{
    public interface IEmailInteraction
    {
        Task<Response<bool>> SendEmail(EmailModel emailModel);
    }
}
