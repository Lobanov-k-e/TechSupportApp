using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Models;
using TechSupportApp.Application.Interfaces;

namespace TechSupportApp.Application.Tickets.Queries.GetTicketsForUser
{
    public class GetTicketsForUser : IRequest<GetTicketsForUserVm>
    {
        public string CurrentUserIdentity { get; set; }
    }

    internal class GetTicketsForUserHandler : IRequestHandler<GetTicketsForUser, GetTicketsForUserVm>
    {
        private readonly IMapper _mapper;
        private readonly IAppContext _context;
        private readonly IIdentityService _identityService;

        public GetTicketsForUserHandler(IMapper mapper, IAppContext context, IIdentityService identityService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<GetTicketsForUserVm> Handle(GetTicketsForUser request, CancellationToken cancellationToken)
        {
            //consider high priority exception: domain id found, user not found
            (Result result, int domainId) = await _identityService.GetDomainIdAsync(request.CurrentUserIdentity);

            if (!result.Succeeded)
            {
                throw new ApplicationException(string.Concat(result.Errors));
            }
            
            var tickets = await _context
                .Tickets
                .Where(t => t.Issuer.Id.Equals(domainId))
                .ProjectTo<TicketDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new GetTicketsForUserVm
            {
                Tickets = tickets
            };
        }
    }
}

