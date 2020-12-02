using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Queries
{
    public class GetAllTickets : IRequest<IEnumerable<Ticket>>
    {
    }

    internal class GetAllTicketsHandler : IRequestHandler<GetAllTickets, IEnumerable<Ticket>>
    {
        private readonly IAppContext _context;

        public GetAllTicketsHandler(IAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Ticket>> Handle(GetAllTickets request, CancellationToken cancellationToken)
        {
            var tickets = await _context.Tickets.ToListAsync();
            return tickets;
        }
    }
}
