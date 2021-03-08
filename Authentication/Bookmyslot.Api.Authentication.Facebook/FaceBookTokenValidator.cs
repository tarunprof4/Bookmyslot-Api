using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Configuration;
using Bookmyslot.Api.Authentication.Common.Constants;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Authentication.Facebook.Configuration;
using Bookmyslot.Api.Authentication.Facebook.Contracts;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Facebook
{
    public class FaceBookTokenValidator : IFacebookTokenValidator
    {
        private readonly FacebookAuthenticationConfiguration facebookAuthenticationConfiguration;
        private readonly IHttpClientFactory httpClientFactory;

        public FaceBookTokenValidator(IHttpClientFactory httpClientFactory, FacebookAuthenticationConfiguration facebookAuthenticationConfiguration)
        {
            this.httpClientFactory = httpClientFactory;
            this.facebookAuthenticationConfiguration = facebookAuthenticationConfiguration;
        }
        public async Task<Response<SocialCustomerModel>> ValidateToken(string token)
        {
            try
            {
                var validateTokenUrl = string.Format(this.facebookAuthenticationConfiguration.TokenValidationUrl, token, this.facebookAuthenticationConfiguration.ClientId, this.facebookAuthenticationConfiguration.ClientSecret);
                var userInfoUrl = string.Format(this.facebookAuthenticationConfiguration.UserInfoUrl, token);



             

                return new Response<SocialCustomerModel>() { Result = CreateSocialCustomerModel(validPayload) };
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return Response<SocialCustomerModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
            }
        }

        private SocialCustomerModel CreateSocialCustomerModel()
        {
            return new SocialCustomerModel()
            {
                //FirstName = payload.GivenName,
                //LastName = payload.FamilyName,
                //Email = payload.Email,
                Provider = LoginConstants.ProviderFacebook
            };
        }

        public async Task<Response<bool>> ValidateAccessToken(string accessToken)
        {
            var httpClient = httpClientFactory.CreateClient(ApiClient.CustomerApiClient);
            var request = new HttpRequestMessage(HttpMethod.Get, email);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationTokenSource.Token))
            {
                if (!response.IsSuccessStatusCode)
                {
                    //return await response.HandleError<ProfileSettings>();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var facebookTokenValidationResponse = stream.ReadAndDeserializeFromJson<FacebookTokenValidation>();

                return new Response<bool>() { Result = facebookTokenValidationResponse };
            }

        }


        private async Task<Response<T>> HandleError<T>(this HttpResponseMessage httpResponseMessage)
        {
            var errorStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var errors = errorStream.ReadAndDeserializeFromJson<List<string>>();

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Response<T>.Empty(errors);
            }

            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Response<T>.ValidationError(errors);
            }

            return Response<T>.Failed(errors);
        }
    }
}
