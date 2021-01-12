using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Infrastructure.Identity
{
    class UserService : IUserService
    {
        //facade and anti-corruption layer for asp.net core identity
        //provides projections between identity and domain users
        
        
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ApplicationContext _context;

        public UserService(UserManager<AppIdentityUser> userManager, ApplicationContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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
        public async Task<bool> CreateAsync(string name, string email, string password, int domainId)
        {
            AppIdentityUser user = new AppIdentityUser
            {
                UserName = name,
                Email = email,
                DomainId = domainId
            };
            IdentityResult result = await _userManager.CreateAsync(user, password);

            return result.Succeeded;
        }
    }
}
