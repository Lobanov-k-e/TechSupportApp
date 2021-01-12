using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechSupportApp.Infrastructure.Identity
{
    class AppIdentityUser : IdentityUser
    {
        public int DomainId { get; set; }
    }
}
