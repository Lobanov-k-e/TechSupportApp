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
    public class AddMessage : IRequest
    {
        public int TicketId { get; set; }  
        public string Content { get; set; }
    }

    internal class AddMessageHandler : IRequestHandler<AddMessage>
    {
        private readonly IAppContext _context;

        public AddMessageHandler(IAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Unit> Handle(AddMessage request, CancellationToken cancellationToken)
        {
            //убрать инклюд, посмотреть что будет
            var ticket = await _context
                .Tickets
                .Include(t => t.Track)
                .SingleOrDefaultAsync(t => t.Id == request.TicketId)
                ?? throw new NotFoundException(name: nameof(Ticket), request.TicketId);

            ticket.AddMessage(request.Content);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
