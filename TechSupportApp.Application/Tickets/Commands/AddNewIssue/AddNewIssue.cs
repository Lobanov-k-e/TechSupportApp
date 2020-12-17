using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Commands.AddIssue
{
    public class AddNewIssue : IRequest
    {
        public int TicketId { get; set; }
        public string Issue { get; set; }
    }

    public class AddNewIssueRequestHandler : IRequestHandler<AddNewIssue>
    {
        private readonly IAppContext _context;

        public AddNewIssueRequestHandler(IAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Unit> Handle(AddNewIssue request, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets.FindAsync(request.TicketId) ??
                         throw new NotFoundException(name: nameof(Ticket), key: request.TicketId);

            ticket.AddIssue(request.Issue);
            
            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
