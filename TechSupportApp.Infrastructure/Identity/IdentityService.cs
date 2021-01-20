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
        private readonly ApplicationContext _context;

        public IdentityService(UserManager<AppIdentityUser> userManager, ApplicationContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            //using context directly might not be the best idea. use commands?
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /*
         * consider:
         * probably should return projection of 
         * identity to domain user id, not user inself
         * also works better if identity not found
         */
        public async Task<User> GetUserByIdentity(string name)
        {
            var userIdentity = await _userManager.FindByIdAsync(name);

            var user = await _context.Users.FindAsync(userIdentity?.DomainId);

            return user;
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
