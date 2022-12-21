

using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Constants;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Authentication.Google.Configuration;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.SharedKernel.Contracts.Logging;
using Bookmyslot.SharedKernel.ValueObject;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Google
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        private readonly GoogleAuthenticationConfiguration googleAuthenticationConfiguration;
        private readonly ILoggerService loggerService;

        public GoogleTokenValidator(GoogleAuthenticationConfiguration googleAuthenticationConfiguration, ILoggerService loggerService)
        {
            this.googleAuthenticationConfiguration = googleAuthenticationConfiguration;
            this.loggerService = loggerService;
        }
        public async Task<Result<SocialCustomerModel>> ValidateToken(string idToken)
        {
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings();
                validationSettings.Audience = new List<string>() { this.googleAuthenticationConfiguration.GoogleClientId };
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(idToken, validationSettings);

                return new Result<SocialCustomerModel>() { Value = CreateSocialCustomerModel(validPayload) };
            }
            catch (Exception ex)
            {
                this.loggerService.Error(ex, string.Empty);
                return Result<SocialCustomerModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
            }
        }


        private SocialCustomerModel CreateSocialCustomerModel(GoogleJsonWebSignature.Payload payload)
        {
            return new SocialCustomerModel()
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                Provider = LoginConstants.ProviderGoogle
            };
        }
    }
}
