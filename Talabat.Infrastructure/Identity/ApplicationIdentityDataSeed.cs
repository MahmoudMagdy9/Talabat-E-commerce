using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastructure.Identity
{
    public class ApplicationIdentityDataSeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Mahmoud Magdy",
                    Email = "mahmoud.magdy@talabat.com",
                    UserName = "mahmoud.magdy",
                    PhoneNumber = "01000000000"
                };
                await userManager.CreateAsync(user,"P@ssw0rd");
            }
        }
    }
}