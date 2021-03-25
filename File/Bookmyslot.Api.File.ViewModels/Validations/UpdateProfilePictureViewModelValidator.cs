using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.File.Contracts.Constants;
using Bookmyslot.Api.File.Contracts.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Bookmyslot.Api.File.ViewModels.Validations
{

    public class UpdateProfilePictureViewModelValidator : AbstractValidator<UpdateProfilePictureViewModel>
    {
        private readonly IFileConfigurationBusiness fileConfigurationBusiness;
        public UpdateProfilePictureViewModelValidator(IFileConfigurationBusiness fileConfigurationBusiness)
        {
            this.fileConfigurationBusiness = fileConfigurationBusiness;
            RuleFor(x => x.ImageFile).Cascade(CascadeMode.Stop).Must(isFileNull).WithMessage(AppBusinessMessagesConstants.FileMissing).
                Must(isFileSizeValid).WithMessage(AppBusinessMessagesConstants.ImageSizeTooLong).
                Must(isImageExtensionValid).WithMessage(AppBusinessMessagesConstants.InvalidImageExtension).
                Must(isImageExtensionSignatureValid).WithMessage(AppBusinessMessagesConstants.InvalidImageExtensionSignature); 
        }



        private bool isFileNull(IFormFile file)
        {
            if (file == null)
            {
                return true;
            }
            return false;
        }


        private bool isFileSizeValid(IFormFile file)
        {
            if (file.Length > 0 && file.Length < ImageConstants.MaxImageSize)
            {
                return true;
            }
            return false;
        }


        private bool isImageExtensionValid(IFormFile file)
        {
            var imageConfiguration = this.fileConfigurationBusiness.GetImageConfigurationInformation();
            return imageConfiguration.isImageExtensionValid(file);
        }

        private bool isImageExtensionSignatureValid(IFormFile file)
        {
            var imageConfiguration = this.fileConfigurationBusiness.GetImageConfigurationInformation();
            return imageConfiguration.isImageExtensionSignatureValid(file);
        }


       

    }
}
