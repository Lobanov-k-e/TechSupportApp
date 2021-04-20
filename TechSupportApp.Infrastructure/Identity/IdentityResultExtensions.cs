using Microsoft.AspNetCore.Identity;
using System.Linq;
using TechSupportApp.Application.Common.Models;

namespace TechSupportApp.Infrastructure.Identity
{
    static class IdentityResultExtensions
    {
        /// <summary>
        /// Converts IdentityResult to ApplicationResult
        /// </summary>        
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }

        /// <summary>
        /// Merge IdentityResluts and converts it to ApplicationReslut 
        /// </summary>      
        public static Result ToApplicationResult(this IdentityResult result0, IdentityResult result1)
        {
            if (!(result0.Succeeded && result1.Succeeded))
            {
                var errors = result0.Errors.ToList();
                errors.AddRange(result1.Errors);

                return Result.Failure(errors.Select(e => e.Description));
            }

            return Result.Success();
        }
    }
}
