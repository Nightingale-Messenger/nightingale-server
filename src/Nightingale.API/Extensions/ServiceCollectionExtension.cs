using System;
using System.Text;
using System.Threading.Tasks;
using Nightingale.Infrastructure.Data;
using Nightingale.App.Models;
using Nightingale.Core.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                        builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                });
        
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration Configuration) =>
            services
                .AddDbContext<NightingaleContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DatabaseContext"))
                        .EnableSensitiveDataLogging()
                );
        
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
                options.ExpireTimeSpan = TimeSpan.FromDays(28);

                options.LoginPath = "/api/auth/login";
                options.SlidingExpiration = true;
            });
        
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
        
        public static IServiceCollection AddUserService(this IServiceCollection services) =>
            services
                .AddTransient<IUserService, UserService>();
    }
}