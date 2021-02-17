using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication
{
    public class SocialLoginTokenValidator : ISocialLoginTokenValidator
    {
        private readonly ITokenValidator tokenValidator;
        public SocialLoginTokenValidator(ITokenValidator tokenValidator)
        {
            this.tokenValidator = tokenValidator;
        }

        public async Task<Response<SocialCustomerModel>> ValidateToken(string token)
        {
            var isTokenValidResponse = await this.tokenValidator.ValidateToken(token);
            return isTokenValidResponse;
        }
    }
}
