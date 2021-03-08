using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Constants;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Authentication.Facebook.Configuration;
using Bookmyslot.Api.Authentication.Facebook.Contracts;
using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Marvin.StreamExtensions;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Authentication.Facebook
{
    public class FaceBookTokenValidator : IFacebookTokenValidator
    {
        private readonly FacebookAuthenticationConfiguration facebookAuthenticationConfiguration;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly CancellationTokenSource cancellationTokenSource =
           new CancellationTokenSource();


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


                var isTokenValid = await this.ValidateAccessToken(validateTokenUrl);
                var facebookUserInfo = await this.GetUserInfo(userInfoUrl);


                return new Response<SocialCustomerModel>() { Result = CreateSocialCustomerModel(facebookUserInfo.Result) };
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return Response<SocialCustomerModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
            }
        }



        //public async Task<Response<bool>> ValidateAccessToken(string url)
        //{
        //    var result = await httpClientFactory.CreateClient().GetAsync(url);
        //    var responseAsString = await result.Content.ReadAsStringAsync();
        //    var facebookTokenValidationResponse = JsonConvert.DeserializeObject<FacebookTokenValidation>(responseAsString);
        //    return new Response<bool>() { Result = facebookTokenValidationResponse.Data.IsValid };

        //}

        //public async Task<Response<FacebookUserInfo>> GetUserInfo(string url)
        //{
        //    var result = await httpClientFactory.CreateClient().GetAsync(url);
        //    var responseAsString = await result.Content.ReadAsStringAsync();
        //    var facebookUserInfo = JsonConvert.DeserializeObject<FacebookUserInfo>(responseAsString);
        //    return new Response<FacebookUserInfo>() { Result = facebookUserInfo };
        //}

        public async Task<Response<bool>> ValidateAccessToken(string url)
        {
            var httpClient = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationTokenSource.Token))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return await response.HandleError<bool>();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var facebookTokenValidationResponse = stream.ReadAndDeserializeFromJson<FacebookTokenValidation>();

                return new Response<bool>() { Result = facebookTokenValidationResponse.Data.IsValid };
            }

        }

        public async Task<Response<FacebookUserInfo>> GetUserInfo(string url)
        {
            var httpClient = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationTokenSource.Token))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return await response.HandleError<FacebookUserInfo>();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var facebookUserInfo = stream.ReadAndDeserializeFromJson<FacebookUserInfo>();

                return new Response<FacebookUserInfo>() { Result = facebookUserInfo };
            }

        }


        private SocialCustomerModel CreateSocialCustomerModel(FacebookUserInfo facebookUserInfo)
        {
            return new SocialCustomerModel()
            {
                FirstName = facebookUserInfo.FirstName,
                LastName = facebookUserInfo.LastName,
                Email = facebookUserInfo.Email,
                Provider = LoginConstants.ProviderFacebook
            };
        }
      
    }
}
