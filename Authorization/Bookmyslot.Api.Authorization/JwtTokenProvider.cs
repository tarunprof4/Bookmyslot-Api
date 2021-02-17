using Bookmyslot.Api.Authorization.Common.Constants;
using Bookmyslot.Api.Authorization.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookmyslot.Api.Authorization
{
    public class JwtTokenProvider: IJwtTokenProvider
    {
        public string GenerateToken(string email)
        {
            var mySecret = JwtTokenConstants.SecretKey;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtTokenConstants.ClaimEmail, email),
                }),
                Expires = DateTime.UtcNow.AddHours(JwtTokenConstants.TokenExpiryHours),
                Issuer = JwtTokenConstants.Issuer,
                Audience = JwtTokenConstants.Audience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
