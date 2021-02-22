using Bookmyslot.Api.Authentication.Common.Configuration;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookmyslot.Api.Authentication
{
    public class JwtTokenProvider: IJwtTokenProvider
    {
        private readonly AuthenticationConfiguration authenticationConfiguration;

        public JwtTokenProvider(AuthenticationConfiguration authenticationConfiguration)
        {
            this.authenticationConfiguration = authenticationConfiguration;
        }
        public string GenerateToken(string email)
        {
            var mySecret = this.authenticationConfiguration.SecretKey;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(this.authenticationConfiguration.ClaimEmail, email),
                }),
                Expires = DateTime.UtcNow.AddHours(this.authenticationConfiguration.ExpiryInHours),
                Issuer = this.authenticationConfiguration.Issuer,
                Audience = this.authenticationConfiguration.Audience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
