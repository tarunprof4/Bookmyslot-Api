using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Azure.Services.Constants;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Azure.Services.Storage
{
    public class BlobRepository : IBlobRepository
    {
        private readonly string blobStorageConnectionString;
        private readonly IHashing sha256SaltedHash;
        private readonly IDbInterceptor dbInterceptor;

        public BlobRepository(IHashing sha256SaltedHash, IDbInterceptor dbInterceptor, AppConfiguration appConfiguration)
        {
            this.blobStorageConnectionString = appConfiguration.BlobStorageConnectionString;
            this.sha256SaltedHash = sha256SaltedHash;
            this.dbInterceptor = dbInterceptor;
        }
        public async Task<Response<string>> UpdateProfilePicture(IFormFile file, string customerId, string firstName)
        {
            var containerName = BlobConstants.UploadProfilePictureContainer;
            var blobName = GenerateProfilePictureBlobName(customerId, firstName);
            BlobContainerClient container = new BlobContainerClient(this.blobStorageConnectionString, containerName);
            BlobClient blobClient = container.GetBlobClient(blobName);
            BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = BlobConstants.ImageContentType
            };


            using (var stream = file.OpenReadStream())
            {
                var parameters = new { CustomerId = customerId };
                await this.dbInterceptor.GetQueryResults("SaveProfilePicture", parameters, () => blobClient.UploadAsync(stream, blobHttpHeaders));
            }

            return new Response<string>() { Result = blobClient.Uri.AbsoluteUri };
        }


        private string GenerateProfilePictureBlobName(string customerId, string firstName)
        {
            var hashCustomerId = this.sha256SaltedHash.Create(customerId);
            var blobName = string.Format("{0}{1}", hashCustomerId, firstName);
            return blobName;
        }
    }
}
