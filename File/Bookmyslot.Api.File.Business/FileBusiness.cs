using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.File.Contracts.Constants;
using Bookmyslot.Api.File.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Bookmyslot.Api.File.Business
{
    public class FileBusiness : IFileBusiness
    {
        private readonly IFileConfigurationBusiness fileConfigurationBusiness;
        public FileBusiness(IFileConfigurationBusiness fileConfigurationBusiness)
        {
            this.fileConfigurationBusiness = fileConfigurationBusiness;
        }

        public Response<bool> IsImageValid(IFormFile formFile)
        {
            //if formFile == null
            var imageConfiguration = this.fileConfigurationBusiness.GetImageConfigurationInformation();

            if (formFile.Length > 0 && formFile.Length < ImageConstants.MaxImageSize)
            {
                return ValidateImageExtension(formFile, imageConfiguration);
            }

            return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.ImageSizeTooLong });
        }

        private Response<bool> ValidateImageExtension(IFormFile formFile, Contracts.Configuration.ImageConfigurationSingleton imageConfiguration)
        {
            var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !imageConfiguration.ExtensionSignatures.ContainsKey(ext))
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.InvalidImageExtension });
            }


            var isSignatureValid = MatchFileExtensionSignatures(imageConfiguration.ExtensionSignatures, formFile, ext);
            if (!isSignatureValid)
            {
                return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.InvalidImageExtensionSignature });
            }

            return new Response<bool>() { Result = true };
        }

        private bool MatchFileExtensionSignatures(ReadOnlyDictionary<string, List<byte[]>> fileSignatures, IFormFile formFile, string ext)
        {
            using (var stream = formFile.OpenReadStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    var signatures = fileSignatures[ext];
                    var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                    return signatures.Any(signature =>
                        headerBytes.Take(signature.Length).SequenceEqual(signature));
                }
            }
        }
    }
}
