using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Azure.Repositories.Constants;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Configuration;
using Bookmyslot.Api.Common.Database.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Azure.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private readonly string blobStorageConnectionString;
        private readonly IHashing md5Hash;
        private readonly IDbInterceptor dbInterceptor;

        public BlobRepository(IHashing md5Hash, IDbInterceptor dbInterceptor, AppConfiguration appConfiguration)
        {
            this.blobStorageConnectionString = appConfiguration.BlobStorageConnectionString;
            this.md5Hash = md5Hash;
            this.dbInterceptor = dbInterceptor;
        }
        public async Task<Response<string>> UpdateProfilePicture(IFormFile file, string customerId)
        {
            var containerName = BlobConstants.UploadProfilePictureContainer;
            var blobName = GenerateProfilePictureBlobName(customerId);
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

            return new Response<string>() { Result = blobClient.Uri.AbsoluteUri};
        }


        private string GenerateProfilePictureBlobName(string customerId)
        {
            var hashCustomerId = this.md5Hash.Create(customerId);
            return hashCustomerId;
        }
    }
}
