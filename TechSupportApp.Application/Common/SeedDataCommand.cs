using MediatR;
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

        public SeedDataCommandHandler(IAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override async Task Handle(SeedDataCommand request, CancellationToken cancellationToken)
        {
            var tickets = Enumerable.Range(1, 10).Select(i => Ticket.Create($"body{i}", $"issuer{i}")).ToList();
            await _context.Tickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync();
        }
    }
}
