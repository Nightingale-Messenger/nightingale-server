using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Nightingale.API.Models;
using Nightingale.Core.Entities;

namespace Nightingale.API.Services
{
    public interface IJwtService
    {
        public TokensResult GenerateTokens(string email, Claim[] claims);
        public ClaimsPrincipal GetClaimsFromExpiredToken(string expiredToken);

        public JwtSecurityToken ValidateToken(string token);
    }
    
    public class JwtService : IJwtService
    {
        private JwtConfig _jwtConfig;
        
        public JwtService(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public TokensResult GenerateTokens(string email, Claim[] claims)
        {
            var accessToken = new JwtSecurityToken(
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
                claims,
                expires: DateTime.Now.AddHours(_jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                        _jwtConfig.Secret)), SecurityAlgorithms.HmacSha256Signature));
            
            return new TokensResult()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = GenerateRefreshToken(email)
            };
        }

        public ClaimsPrincipal GetClaimsFromExpiredToken(string expiredToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
                ValidateLifetime = false
            };
            SecurityToken securityToken;
            return new JwtSecurityTokenHandler().ValidateToken(expiredToken, tokenValidationParameters,
                out securityToken);
        }

        public JwtSecurityToken ValidateToken(string token)
        {
            new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

        private RefreshToken GenerateRefreshToken(string Email)
        {
            var random = new Random();
            return new RefreshToken()
            {
                Email = Email,
                ExpirationDate = DateTime.Now.AddHours(_jwtConfig.RefreshTokenExpiration),
                Token = GenerateRefreshTokenString()
            };
        }

        private string GenerateRefreshTokenString()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}