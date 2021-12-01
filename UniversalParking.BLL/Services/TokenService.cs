using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UniversalParking.BLL.DTO;
using UniversalParking.BLL.Interfaces;
using UniversalParking.DAL.Entities;
using System;   
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.BLL.Services
{
    public class TokenService : ITokenService
    {
        private string secretKey;
        private string issuer;
        private string audience;

        public TokenService(IConfiguration configuration)
        {
            var jwtParametrs = configuration.GetSection("JWT");
            this.secretKey = jwtParametrs["Key"];
            this.issuer = jwtParametrs["Issuer"];
            this.audience = jwtParametrs["Audience"];
        }

        public string GenerateToken(int userID, string role)
        {

            var mySecurityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, userID.ToString()),
                new(ClaimTypes.Role, role),
            };
            //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims, "Token",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(mySecurityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateCurrentToken(string token)
        {
            var mySecurityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }
    }
}
