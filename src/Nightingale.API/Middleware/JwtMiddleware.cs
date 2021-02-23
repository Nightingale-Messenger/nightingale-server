using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Nightingale.API.Services;
using Nightingale.App.Interfaces;

namespace Nightingale.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,
            IUserService userService,
            IJwtService jwtService)
        {
            var token = await context.GetTokenAsync("Bearer", "access_token");
            if (token != null)
            {
                AttachUserToHttpContext(context, jwtService, token);
            }

            await _next(context);
        }

        private async void AttachUserToHttpContext(HttpContext context,
            IJwtService jwtService,
            string token)
        {
            try
            { ;
                var validatedToken = jwtService.ValidateToken(token);
                await context.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(
                        validatedToken.Claims, "basic")));
            }
            catch
            {
                // ignored
            }
        }
    }
}