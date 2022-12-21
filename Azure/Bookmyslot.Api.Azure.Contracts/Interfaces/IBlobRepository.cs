using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Azure.Contracts.Interfaces
{
    public interface IBlobRepository
    {
        Task<Result<string>> UpdateProfilePicture(IFormFile file, string customerId, string firstName);
    }
}
