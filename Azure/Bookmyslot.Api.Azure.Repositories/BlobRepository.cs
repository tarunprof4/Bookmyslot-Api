using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Azure.Repositories
{
    public class BlobRepository : IBlobRepository
    {

        public async Task<Response<string>> SaveProfilePicture(IFormFile file, string blobName)
        {
            var containerName = "image";
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            BlobClient blob = container.GetBlobClient(fileName);
            BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = "image/jpeg";
            var isMatch = MatchExtension(for, ext) ;


            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, blobHttpHeaders);
            }

            return new Response<string>() { Result = "" };
        }
    }
}
