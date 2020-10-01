using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Utils
{
    public static class TokenCreator
    {
        public static string CreateToken(IConfiguration options, User user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim("Email", user.Email),
                new Claim("Username", user.UserName)
            };
            var jwt = new JwtSecurityToken(
                issuer: options["ISSUER"],
                audience: options["AUDIENCE"],
                notBefore: DateTime.UtcNow,
                claims: userClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options["KEY"])),
                    SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}