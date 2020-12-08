using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;


namespace TechSupportApp.Application.Tickets.Queries.GetAllTickets
{
    public class GetAllTickets : IRequest<GetAllTicketsVm>
    {
    }

    //не возвращать доменный объект, использовать автомэпер
    internal class GetAllTicketsHandler : IRequestHandler<GetAllTickets, GetAllTicketsVm>
    {
        private readonly IAppContext _context;
        private readonly IMapper _mapper;

        public GetAllTicketsHandler(IAppContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<GetAllTicketsVm> Handle(GetAllTickets request, CancellationToken cancellationToken)
        {
            var tickets = await _context.Tickets
                .ProjectTo<TicketDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var vm = new GetAllTicketsVm()
            {
                Tickets = tickets
            };

            return vm;
        }
    }
}
