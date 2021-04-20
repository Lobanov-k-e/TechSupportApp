using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TechSupportApp.Application.Mappings;
using TechSupportApp.Application.Tickets.Queries.GetAllTickets;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Tests.Application.Tickets.Queries
{
    [TestFixture]
    class GetAllTicketsTests
    {
        private ApplicationContext _context;
        public GetAllTicketsTests()
        {
            _context = DBContextFactory.Create();
        }

        [Test]
        public async Task GetAllTickets_Gets_All()
        {
            var handler = new GetAllTicketsHandler(_context, GetMapper());
            var result = await handler.Handle(new GetAllTickets(), new System.Threading.CancellationToken());

            CollectionAssert.IsNotEmpty(result.Tickets);
            Assert.AreEqual(_context.Tickets.Count(), result.Tickets.Count());
        }

        [Test]
        public async Task GetsCorrectTickets()
        {
            var handler = new GetAllTicketsHandler(_context, GetMapper());
            var result = await handler.Handle(new GetAllTickets(), new System.Threading.CancellationToken());

            var expected = await _context.Tickets.Select(t => new TicketDTO{
                Id = t.Id,
                Issue = t.Issue,
                Issuer = t.Issuer.Name,
                TicketStatus = (int)t.TicketStatus
            }).ToListAsync();

            CollectionAssert.AreEqual(expected, result.Tickets, new TicketDTOComparer() );           
        }

        private IMapper GetMapper()
        {

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return configurationProvider.CreateMapper();
        }

        private class TicketDTOComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var t1 = x as TicketDTO;
                var t2 = y as TicketDTO;

                if (t1.Id == t2.Id 
                    && t1.Issue == t2.Issue 
                    && t1.Issuer == t2.Issuer 
                    && t1.TicketStatus == t2.TicketStatus)
                {
                    return 0;
                }

                return -1;
            }
        }
    }
}
