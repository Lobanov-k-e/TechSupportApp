using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Commands.CloseTicket
{
    public class CloseTicket : IRequest
    {
        public int TicketId { get; set; }
    }

    internal class CloseTicketHandler : IRequestHandler<CloseTicket>
    {
        private readonly IAppContext _context;

        public CloseTicketHandler(IAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Unit> Handle(CloseTicket request, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets.FindAsync(request.TicketId)
                ?? throw new NotFoundException(name: nameof(Ticket), key: request.TicketId);

            ticket.Close();
            await _context.SaveChangesAsync();

            return Unit.Value;            
        }
    }
}
