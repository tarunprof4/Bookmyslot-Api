using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Configuration;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Facebook
{
    public class FaceBookTokenValidator : IFacebookTokenValidator
    {
        private readonly AuthenticationConfiguration authenticationConfiguration;

        public FaceBookTokenValidator(AuthenticationConfiguration authenticationConfiguration)
        {
            this.authenticationConfiguration = authenticationConfiguration;
        }
        public Task<Response<SocialCustomerModel>> ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
