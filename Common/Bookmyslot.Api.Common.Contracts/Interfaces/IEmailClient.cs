using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Contracts.Interfaces
{
    public interface IEmailClient
    {
        Task<Response<bool>> SendEmail(EmailModel emailModel);
    }
}
