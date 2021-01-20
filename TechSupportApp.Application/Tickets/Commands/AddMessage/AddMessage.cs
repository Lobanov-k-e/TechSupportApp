using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
        public string UserId { get; set; }

    }

    internal class AddMessageHandler : IRequestHandler<AddMessage>
    {
        private readonly IAppContext _context;
        private readonly IIdentityService _service;

        public AddMessageHandler(IAppContext context, IIdentityService service)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
        public async Task<Unit> Handle(AddMessage request, CancellationToken cancellationToken)
        {
            //убрать инклюд, посмотреть что будет
            var ticket = await _context
                .Tickets
                .Include(t => t.Track)
                .SingleOrDefaultAsync(t => t.Id == request.TicketId)
                ?? throw new NotFoundException(name: nameof(Ticket), request.TicketId);

            var user = await _service.GetUserByIdentity(request.UserId)
                ?? throw new NotFoundException(name: nameof(User), key: request.UserId); 

            ticket.AddMessage(request.Content, user);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
