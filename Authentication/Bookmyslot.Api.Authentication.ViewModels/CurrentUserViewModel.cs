using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Common.Contracts;

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


        public static Response<CurrentUserViewModel> CreateCurrentUserViewModel(CurrentUserModel currentUserModel)
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

            var currentUserViewModelResponse = new Response<CurrentUserViewModel>() { Result = currentUserViewModel };
            return currentUserViewModelResponse;
        }
    }
}
