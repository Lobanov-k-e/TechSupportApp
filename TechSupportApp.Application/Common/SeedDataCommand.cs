using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Common
{
    public class SeedDataCommand : IRequest
    {
    }

    internal class SeedDataCommandHandler : AsyncRequestHandler<SeedDataCommand>
    {
        private readonly IAppContext _context;
        private readonly IIdentityService _identityService;

        public SeedDataCommandHandler(IAppContext context, IIdentityService identityService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        protected override async Task Handle(SeedDataCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Tickets.CountAsync() != 0)            
                return;

            var users = Enumerable
                .Range(1, 10)
                .Select(i => new User() { Name = $"user{i}" })
                .ToList();

            var tickets = Enumerable
                .Range(1, 10)
                .Select(i => Ticket.Create($"body{i}", users[i - 1]))
                .ToList();

            for (int i = 0; i < tickets.Count; i++)
            {
                var ticket = tickets[i];
                ticket.AddMessage("testMsg", users[i]);
            }

            await _context.Tickets.AddRangeAsync(tickets);           

            await _context.SaveChangesAsync();

            var createdUsers = await _context.Users.ToListAsync();            

            await CreateCredentials(createdUsers);

        }      

        private async Task CreateCredentials(List<User> users)
        {
            foreach (var user in users)
            {
                await _identityService.CreateAsync(user.Name, $"{user.Name}@test.com", "Secret123$", user.Id);
            }
        }
    }
}
