using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication
{
    public class SocialLoginTokenValidator : ISocialLoginTokenValidator
    {
        private readonly IGoogleTokenValidator googleTokenValidator;
        private readonly IFacebookTokenValidator facebookTokenValidator;
        public SocialLoginTokenValidator(IGoogleTokenValidator googleTokenValidator, IFacebookTokenValidator facebookTokenValidator)
        {
            this.googleTokenValidator = googleTokenValidator;
            this.facebookTokenValidator = facebookTokenValidator;
        }

        public async Task<Result<SocialCustomerModel>> LoginWithFacebook(string authToken)
        {
            return await this.facebookTokenValidator.ValidateAccessToken(authToken);
        }

        public async Task<Result<SocialCustomerModel>> LoginWithGoogle(string idToken)
        {
            return await this.googleTokenValidator.ValidateToken(idToken);
        }


    }
}
