using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Azure.Repositories.Constants;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Azure.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private readonly string blobConnectionString;
        private readonly IHashing md5Hash;

        public BlobRepository(IHashing md5Hash)
        {
            this.md5Hash = md5Hash;
            this.blobConnectionString = "";
        }
        public async Task<Response<string>> SaveProfilePicture(IFormFile file, string customerId)
        {
            var containerName = BlobConstants.UploadProfilePictureContainer;
            var blobName = GenerateProfilePictureBlobName(customerId);
            BlobContainerClient container = new BlobContainerClient(this.blobConnectionString, containerName);
            BlobClient blobClient = container.GetBlobClient(blobName);
            BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = BlobConstants.ImageContentType
            };


            using (var stream = file.OpenReadStream())
            {
               var response = await blobClient.UploadAsync(stream, blobHttpHeaders);
            }

            return new Response<string>() { Result = "" };
        }


        private string GenerateProfilePictureBlobName(string customerId)
        {
            var hashCustomerId = this.md5Hash.Create(customerId);
            return hashCustomerId;
        }
    }
}
