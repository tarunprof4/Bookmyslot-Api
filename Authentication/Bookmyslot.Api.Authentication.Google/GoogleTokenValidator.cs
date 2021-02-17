

using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Configuration;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Google.Apis.Auth;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Google
{
    public class GoogleTokenValidator : ITokenValidator
    {
        private readonly AuthenticationConfiguration authenticationConfiguration;

        public GoogleTokenValidator(AuthenticationConfiguration authenticationConfiguration)
        {
            this.authenticationConfiguration = authenticationConfiguration;
        }
        public async Task<Response<SocialCustomerModel>> ValidateToken(string token)
        {
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings();
                validationSettings.Audience = new List<string>() { this.authenticationConfiguration.GoogleClientId };
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                return new Response<SocialCustomerModel>() { Result = CreateSocialCustomerModel(validPayload) };
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return Response<SocialCustomerModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.InvalidToken });
            }
        }


        private SocialCustomerModel CreateSocialCustomerModel(GoogleJsonWebSignature.Payload payload)
        {
            return new SocialCustomerModel() { FirstName = payload.GivenName, LastName = payload.FamilyName, 
                Email = payload.Email };
        }
    }
}
