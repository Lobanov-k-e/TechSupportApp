using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Models;
using TechSupportApp.Application.Interfaces;

namespace TechSupportApp.Infrastructure.Identity
{
    //facade and anti-corruption layer for asp.net core identity
    //provides projections between identity and domain users   
    class IdentityService : IIdentityService
    {        
        private const string DefaultRoleName = "NormalUser";

        //а не добавить ли еще один уровень абстракции? все операции с identity убрать в стратегию

        private readonly UserManager<AppIdentityUser> _userManager;
        

        public IdentityService(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));            
        }
              
        public async Task<(Result result, int id)> GetDomainIdAsync(string id)
        {
            var userIdentity = await _userManager.FindByIdAsync(id);

            var result = userIdentity is null
                ? Result.Failure(new string[] { $"no user with identity {id}"}) 
                : Result.Success();

            return (result, userIdentity?.DomainId ?? 0);           
        }

        // validation on application level
        public async Task<(Result result, string userId)> CreateAsync(string name, string email, string password, int domainId)
        {
                     
            AppIdentityUser user = new AppIdentityUser
            {
                UserName = name,
                Email = email,
                DomainId = domainId,                               
            };

            var createUserResult = await _userManager.CreateAsync(user, password);

            if (createUserResult.Succeeded)
            {
                var assignRoleResult = await _userManager.AddToRoleAsync(user, DefaultRoleName);

                return (assignRoleResult.ToApplicationResult(), user.Id);
            }

            return (createUserResult.ToApplicationResult(), user.Id);
        }

       
        public async Task<Result> DeleteAsync(int domainId)
        {
            var user = await _userManager.Users.Where(u => u.DomainId == domainId).FirstOrDefaultAsync();
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();            
        }
    }
}
