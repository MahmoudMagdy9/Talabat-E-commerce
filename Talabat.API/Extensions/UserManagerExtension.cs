using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;

namespace Talabat.API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<ApplicationUser?> FindUserWithAddressAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(U => U.Address)
                .FirstOrDefaultAsync(U => U.Email == Email);
            return user;
        }
    }
}
