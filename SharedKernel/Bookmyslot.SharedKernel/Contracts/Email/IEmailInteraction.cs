using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.SharedKernel.Contracts.Email
{
    public interface IEmailInteraction
    {
        Task<Result<bool>> SendEmail(EmailModel emailModel);
    }
}
