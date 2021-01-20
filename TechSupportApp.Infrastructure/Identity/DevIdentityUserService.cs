using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Infrastructure.Identity
{
    public class DevIdentityUserService : IIdentityService
    {
        private readonly ApplicationContext _context;

        public DevIdentityUserService(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<bool> CreateAsync(string name, string email, string password, int domainId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdentity(string id)
        {
            int trueId = int.Parse(id);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == trueId);

            return user;
        }
    }
}
