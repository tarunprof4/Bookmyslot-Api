using Bookmyslot.Api.Authorization.Common;
using Bookmyslot.Api.Authorization.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authorization
{
    public class SocialLoginTokenValidator : ISocialLoginTokenValidator
    {
        private readonly ITokenValidator tokenValidator;
        public SocialLoginTokenValidator(ITokenValidator tokenValidator)
        {
            this.tokenValidator = tokenValidator;
        }

        public async Task<Response<SocialCustomer>> ValidateToken(string token)
        {
            var isTokenValidResponse = await this.tokenValidator.ValidateToken(token);
            return isTokenValidResponse;
        }
    }
}
