using Bookmyslot.Api.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Azure.Contracts.Interfaces
{
    public interface IBlobRepository
    {
        Task<Response<string>> UpdateProfilePicture(IFormFile file, string blobName);
    }
}
