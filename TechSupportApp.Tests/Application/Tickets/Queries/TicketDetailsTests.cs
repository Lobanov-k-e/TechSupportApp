using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Mappings;
using TechSupportApp.Application.Tickets.Queries.TicketDetails;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Tests.Application.Tickets.Queries
{
    [TestFixture]
    class TicketDetailsTests
    {
        private ApplicationContext _context;
        public TicketDetailsTests()
        {
            _context = DBContextFactory.Create();
        }

        [Test]
        public async Task CanGetTicket()
        {
            var ticket = await _context.Tickets.FirstAsync();

            var query = new TicketDetails() { Id = ticket.Id };
            var handler = new TicketDetailsHandler(_context, GetMapper());

            var result = await handler.Handle(query, new System.Threading.CancellationToken());

            Assert.IsNotNull(result as TicketDetailsVm);
            Assert.AreEqual(ticket.Id, result.Ticket.Id);
        }

        [Test]
        public async Task Throws_onWrongIdn()
        {
            const int InvalidId = -1;
            string errorMsg = $"Entity \"Ticket\" ({InvalidId}) was not found.";
            var query = new TicketDetails() { Id = InvalidId };
            var handler = new TicketDetailsHandler(_context, GetMapper());            

            var act = Assert.ThrowsAsync<NotFoundException>(async() => 
                            await handler.Handle(query, new System.Threading.CancellationToken()));

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }
        

        //create base class for tests
        private IMapper GetMapper()
        {

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return configurationProvider.CreateMapper();
        }

    }
}
