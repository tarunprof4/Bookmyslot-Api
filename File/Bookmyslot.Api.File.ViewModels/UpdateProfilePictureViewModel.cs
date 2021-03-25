using Microsoft.AspNetCore.Http;
using System;

namespace Bookmyslot.Api.File.ViewModels
{
    public class UpdateProfilePictureViewModel
    {
        public IFormFile ImageFile { get; set; }

        public UpdateProfilePictureViewModel(IFormFile file)
        {
            this.ImageFile = file;
        }
    }
}
