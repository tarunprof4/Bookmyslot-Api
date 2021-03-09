using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Constants;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Authentication.Facebook.Configuration;
using Bookmyslot.Api.Authentication.Facebook.Contracts;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Marvin.StreamExtensions;
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

                var isTokenValidResponse = this.ValidateAccessToken(validateTokenUrl);
                var facebookUserInfoResponse = this.GetUserInfo(userInfoUrl);
                await Task.WhenAll(isTokenValidResponse, facebookUserInfoResponse);

                var isTokenValid = isTokenValidResponse.Result;
                var facebookUserInfo = facebookUserInfoResponse.Result;
                if (isTokenValid.ResultType == ResultType.Success && facebookUserInfo.ResultType == ResultType.Success)
                {
                    return new Response<SocialCustomerModel>() { Result = CreateSocialCustomerModel(facebookUserInfo.Result) };
                }

                return Response<SocialCustomerModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return Response<SocialCustomerModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
            }
        }

        public async Task<Response<bool>> ValidateAccessToken(string url)
        {
            var httpClient = httpClientFactory.CreateClient();
            HttpRequestMessage request = CreateHttpRequest(url);

            using (var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationTokenSource.Token))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Log.Error("FaceBook Validate Access Token Failed");
                    return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var facebookTokenValidationResponse = stream.ReadAndDeserializeFromJson<FacebookTokenValidation>();

                if (facebookTokenValidationResponse.Data.IsValid)
                {
                    return new Response<bool>() { Result = true };
                }
                else
                {
                    Log.Error("FaceBook Validate Access Token {@errorStream}", stream);
                    return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
                }
            }

        }

        public async Task<Response<FacebookUserInfo>> GetUserInfo(string url)
        {
            var httpClient = httpClientFactory.CreateClient();
            HttpRequestMessage request = CreateHttpRequest(url);

            using (var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationTokenSource.Token))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                if (!response.IsSuccessStatusCode)
                {
                    Log.Error("FaceBook Get User Info Failed {@errorStream}", stream);
                    return Response<FacebookUserInfo>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
                }

                var facebookUserInfo = stream.ReadAndDeserializeFromJson<FacebookUserInfo>();
                return new Response<FacebookUserInfo>() { Result = facebookUserInfo };
            }
        }

        private static HttpRequestMessage CreateHttpRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            return request;
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
