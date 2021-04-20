using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechSupportApp.Infrastructure.Identity
{
    public class AppIdentityUser : IdentityUser
    {
        public int DomainId { get; set; }
    }
}
