using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.SharedKernel.ValueObject;

namespace Bookmyslot.Api.Authentication.ViewModels
{

    public class CurrentUserViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BioHeadLine { get; set; }

        public bool IsVerified { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string UserName { get; set; }


        public static Result<CurrentUserViewModel> CreateCurrentUserViewModel(CurrentUserModel currentUserModel)
        {
            var currentUserViewModel = new CurrentUserViewModel
            {
                FirstName = currentUserModel.FirstName,
                LastName = currentUserModel.LastName,
                BioHeadLine = currentUserModel.BioHeadLine,
                IsVerified = currentUserModel.IsVerified,
                ProfilePictureUrl = currentUserModel.ProfilePictureUrl,
                UserName = currentUserModel.UserName
            };

            var currentUserViewModelResponse = new Result<CurrentUserViewModel>() { Value = currentUserViewModel };
            return currentUserViewModelResponse;
        }
    }
}
