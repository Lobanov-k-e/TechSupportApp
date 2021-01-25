using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Models;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Infrastructure.Identity
{
    //facade and anti-corruption layer for asp.net core identity
    //provides projections between identity and domain users   
    class IdentityService : IIdentityService
    {            
        private readonly UserManager<AppIdentityUser> _userManager;
       

        public IdentityService(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));            
        }
              
        public async Task<(Result result, int id)> GetDomainId(string id)
        {
            var userIdentity = await _userManager.FindByIdAsync(id);

            var result = userIdentity is null
                ? Result.Failure(new string[] { $"no user with identity{id}" }) 
                : Result.Success();

            return (result, userIdentity.DomainId);           
        }

        // validation on application level
        public async Task<(Result result, string userId)> CreateAsync(string name, string email, string password, int domainId)
        {
            AppIdentityUser user = new AppIdentityUser
            {
                UserName = name,
                Email = email,
                DomainId = domainId
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }
    }
}
