using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Commands.CreateTicket
{
    public class CreateTicket : IRequest<int>
    {
        public string userId { get; set; }
        public string Issue { get; set; }
    }

    internal class CreateTicketRequestHandler : IRequestHandler<CreateTicket, int>
    {
        private readonly IAppContext _context;
        private readonly IUserService _userService;

        public CreateTicketRequestHandler(IAppContext context, IUserService userService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService;
        }
        public async Task<int> Handle(CreateTicket request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdentity(request.userId) 
                ?? throw new NotFoundException(name: nameof(User), key: request.userId);
            var ticket = Ticket.Create(request.Issue, user);

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket.Id;
        }
    }
}
