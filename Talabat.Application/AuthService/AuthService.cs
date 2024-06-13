using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            //private claims (user defined claims)
            var authClaims = new List<Claim>()
             {
                 new Claim(ClaimTypes.Name, user.DisplayName),
                 new Claim(ClaimTypes.Email, user.Email),
             };
            var userRoles = userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles.Result)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse( _configuration["JWT:ExpireTime"] ?? "0") ),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
            );
            
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));



        }
    }
}
