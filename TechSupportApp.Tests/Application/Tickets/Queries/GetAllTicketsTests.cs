using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Tickets.Queries;
using TechSupportApp.Infrastructure;

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
            var handler = new GetAllTicketsHandler(_context);
            var result = await handler.Handle(new GetAllTickets(), new System.Threading.CancellationToken());

            Assert.AreEqual(_context.Tickets.Count(), result.Count());
        }
    }
}
