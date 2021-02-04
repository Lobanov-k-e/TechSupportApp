using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace TechSupportApp.Application.Tickets.Queries.TicketDetails
{
    public class TicketDetails : IRequest<TicketDetailsVm>
    {
        public int Id { get; set; }
    }

    internal class TicketDetailsHandler : IRequestHandler<TicketDetails, TicketDetailsVm>
    {
        private readonly IAppContext _context;
        private readonly IMapper _mapper;

        public TicketDetailsHandler(IAppContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<TicketDetailsVm> Handle(TicketDetails request, CancellationToken cancellationToken)
        {
            var query = _context
                .Tickets              
                .Where(t => t.Id == request.Id)
                .ProjectTo<TicketDTO>(_mapper.ConfigurationProvider);

            var ticket = await query.FirstOrDefaultAsync() ??
                throw new NotFoundException(name: nameof(Ticket), key: request.Id);           

            return new TicketDetailsVm()
            {
                Ticket = ticket
            };

        }
    }
}
