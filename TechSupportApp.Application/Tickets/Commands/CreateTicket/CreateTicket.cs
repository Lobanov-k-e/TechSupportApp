using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Common.Models;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Commands.CreateTicket
{
    public class CreateTicket : IRequest<int>
    {
        public string UserId { get; set; }
        public string Issue { get; set; }
    }

    internal class CreateTicketRequestHandler : IRequestHandler<CreateTicket, int>
    {
        private readonly IAppContext _context;
        private readonly IIdentityService _identityService;

        public CreateTicketRequestHandler(IAppContext context, IIdentityService identityService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _identityService = identityService;
        }
        public async Task<int> Handle(CreateTicket request, CancellationToken cancellationToken)
        {           
            (Result result, int domainId) = await _identityService.GetDomainId(request.UserId);

            if (!result.Succeeded)
            {
                throw new ApplicationException(string.Concat(result.Errors));
            }

            var user = await _context.Users.FindAsync(domainId);

            _ = user ?? throw new NotFoundException(name: nameof(User), key: domainId);

            var ticket = Ticket.Create(request.Issue, user);

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket.Id;
        }
    }
}
