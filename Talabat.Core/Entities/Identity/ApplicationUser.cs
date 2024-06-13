using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Talabat.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {

        public string DisplayName { get; set; } = null!;

        public Address Address { get; set; }
    }
}
