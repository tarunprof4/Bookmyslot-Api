using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
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

        public async Task<Response<SocialCustomerModel>> LoginWithFacebook(string token, string clientSecret)
        {
            return await this.facebookTokenValidator.ValidateToken(token, clientSecret);
        }

        public async Task<Response<SocialCustomerModel>> LoginWithGoogle(string token)
        {
            return await this.googleTokenValidator.ValidateToken(token);
        }

      
    }
}
