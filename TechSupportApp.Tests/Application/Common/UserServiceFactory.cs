using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;
using TechSupportApp.Infrastructure.Identity;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Tests.Application.Common
{
    class UserServiceFactory
    {
        public static IIdentityService Create(IAppContext context)
        {
            return new DevIdentityUserService(context as ApplicationContext);
        }

      
    }
}
