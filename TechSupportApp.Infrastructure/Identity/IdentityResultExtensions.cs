using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechSupportApp.Application.Common.Models;

namespace TechSupportApp.Infrastructure.Identity
{
    static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded 
                ? Result.Success() 
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
