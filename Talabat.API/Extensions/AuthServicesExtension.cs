using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Talabat.Application.AuthService;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.Identity;

namespace Talabat.API.Extensions
{
    public static class AuthServicesExtension
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
             

            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]??"0"))
                };
            });
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            return services;
        }
    }
}
