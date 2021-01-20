using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TechSupportApp.Infrastructure.Identity
{
    class IdentityContext : IdentityDbContext<AppIdentityUser>
    {        
        public IdentityContext([NotNull]DbContextOptions<IdentityContext> options) : base(options)
        {
        }
    }
}
