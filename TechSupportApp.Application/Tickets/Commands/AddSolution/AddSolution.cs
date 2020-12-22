using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Commands
{
    public class AddSolution : IRequest
    {
        public int TicketId { get; set; }
        public int IssueId { get; set; }
        public string Solution { get; set; }
    }

    internal class AddSolutionHandler : IRequestHandler<AddSolution>
    {
        private readonly IAppContext _context;

        public AddSolutionHandler(IAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Unit> Handle(AddSolution request, CancellationToken cancellationToken)
        {
            //убрать инклюд, посмотреть что будет
            var ticket = await _context
                .Tickets
                .Include(t => t.Entries)
                .SingleOrDefaultAsync(t => t.Id == request.TicketId)
                ?? throw new NotFoundException(name: nameof(Ticket), request.TicketId);

            ticket.AddSolution(request.IssueId, request.Solution);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
