using Bookmyslot.Api.Authorization.Common;
using Bookmyslot.Api.Authorization.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class LoginCustomerBusiness : ILoginCustomerBusiness
    {
        private readonly IRegisterCustomerBusiness registerCustomerBusiness;
        private readonly ISocialLoginTokenValidator socialLoginTokenValidator;
        private readonly IJwtTokenProvider jwtTokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class. 
        /// </summary>
        public LoginCustomerBusiness(IRegisterCustomerBusiness registerCustomerBusiness, ISocialLoginTokenValidator socialLoginTokenValidator, IJwtTokenProvider jwtTokenProvider)
        {
            this.registerCustomerBusiness = registerCustomerBusiness;
            this.socialLoginTokenValidator = socialLoginTokenValidator;
            this.jwtTokenProvider = jwtTokenProvider;
        }
        public async Task<Response<string>> LoginSocialCustomer(SocialCustomer socialCustomer)
        {
            var validateTokenResponse = await this.socialLoginTokenValidator.ValidateToken(socialCustomer.Token);
            if (validateTokenResponse.ResultType == ResultType.Success)
            {
                var validatedSocialCustomer = validateTokenResponse.Result;

                var registerCustomerModel = new RegisterCustomerModel() { FirstName = validatedSocialCustomer.FirstName, Email = "c@gmail.com" };
                var registerCustomerResponse = await registerCustomerBusiness.RegisterCustomer(registerCustomerModel);
                if (registerCustomerResponse.ResultType == ResultType.Success)
                {
                    var jwtToken = this.jwtTokenProvider.GenerateToken(registerCustomerModel.Email);
                    return new Response<string>() { Result = jwtToken };
                }

                return Response<string>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
            }

            return Response<string>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
        }
    }
}
