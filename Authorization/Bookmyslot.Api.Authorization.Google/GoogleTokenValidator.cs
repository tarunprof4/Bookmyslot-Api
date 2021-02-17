

using Bookmyslot.Api.Authorization.Common;
using Bookmyslot.Api.Authorization.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Google.Apis.Auth;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authorization.Google
{
    public class GoogleTokenValidator : ITokenValidator
    {
        public async Task<Response<SocialCustomer>> ValidateToken(string token)
        {
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings();
                validationSettings.Audience = new List<string>() { "952200248622-8cn9oq0n1fnp0rjga6vsb9oh67kkkt8s.apps.googleusercontent.com" };
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                
                return new Response<SocialCustomer>() { Result = new SocialCustomer() { FirstName = validPayload.GivenName, LastName = validPayload.FamilyName, Email = validPayload.Email } };
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return Response<SocialCustomer>.ValidationError(new List<string>() { AppBusinessMessagesConstants.InvalidToken });
            }
        }
    }
}
