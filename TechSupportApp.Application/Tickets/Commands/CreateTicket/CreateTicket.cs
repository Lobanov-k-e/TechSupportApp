using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Commands.CreateTicket
{
    public class CreateTicket : IRequest<Ticket>
    {
        public string Issuer { get; set; }
        public string Content { get; set; }
    }

    internal class CreateTicketRequestHandler : IRequestHandler<CreateTicket, Ticket>
    {
        private readonly IAppContext _context;

        public CreateTicketRequestHandler(IAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Ticket> Handle(CreateTicket request, CancellationToken cancellationToken)
        {
            var ticket = Ticket.Create(request.Content, request.Issuer);
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
    }
}
