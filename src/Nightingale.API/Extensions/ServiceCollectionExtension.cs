using System;
using System.Text;
using System.Threading.Tasks;
using Nightingale.Infrastructure.Data;
using Nightingale.Core.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nightingale.API.Models;
using Nightingale.API.Services;
using Nightingale.App.Interfaces;
using Nightingale.App.Services;
using Nightingale.Core.Repositories;
using Nightingale.Infrastructure.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services) =>
            services
                .AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                                            builder => builder.WithOrigins("http://localhost:4200")
                                                .AllowAnyMethod()
                                                .AllowAnyHeader()
                                                .AllowCredentials());
                });
        
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddDbContext<NightingaleContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DatabaseContext"))
                );

        public static AuthenticationBuilder AddCustomJwt(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtConfig:Issuer"],
                        ValidAudience = configuration["JwtConfig:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Secret"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/api/messagehub")))
                                Console.WriteLine("hub");
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        

        public static IdentityBuilder AddCustomIdentity(this IServiceCollection services) =>
            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<NightingaleContext>()
                .AddDefaultTokenProviders();
        
        public static IServiceCollection AddCustomCookies(this IServiceCollection services) =>
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromDays(28);

                options.LoginPath = "/api/auth/login";
                options.SlidingExpiration = true;
            });

        public static IServiceCollection AddCustomJwtService(this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddSingleton<JwtConfig>((ServiceProvider) => configuration.GetSection("JwtConfig").Get<JwtConfig>())
                .AddTransient<IJwtService, JwtService>();

        public static IServiceCollection ConfigureCustomIdentity(this IServiceCollection services) =>
            services
                .Configure<IdentityOptions>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
                    options.User.RequireUniqueEmail = false;
                });
        

        public static IServiceCollection AddMessageRepository(this IServiceCollection services) =>
            services
                .AddTransient<IMessageRepository, MessageRepository>();
        
        public static IServiceCollection AddMessageService(this IServiceCollection services) =>
            services
                .AddTransient<IMessageService, MessageService>();
        
        public static IServiceCollection AddHubService(this IServiceCollection services) =>
            services.AddTransient<IHubService, HubService>();
        
        public static IServiceCollection AddUserService(this IServiceCollection services) =>
            services
                .AddTransient<IUserService, UserService>();

        public static IServiceCollection AddCustomRefreshTokenCleanerService(
            this IServiceCollection services) =>
            services.AddHostedService<RefreshTokenCleanService>();
    }
}