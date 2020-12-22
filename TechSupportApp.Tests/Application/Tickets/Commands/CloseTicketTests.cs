using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Application.Tickets.Commands.CloseTicket;
using TechSupportApp.Domain.Enums;
using System.Threading;
using TechSupportApp.Application.Common.Exceptions;

namespace TechSupportApp.Tests.Application.Tickets.Commands
{
    [TestFixture]
    class CloseTicketTests
    {
        IAppContext _context;
        public CloseTicketTests()
        {
            _context = DBContextFactory.Create();
        }

        [Test]
        public async Task CanCloseTicket()
        {
            var ticket = _context
                .Tickets
                .Where(t => t.TicketStatus == TicketStatus.Open)
                .FirstOrDefault();

            var command = new CloseTicket() {TicketId = ticket.Id };

            await new CloseTicketHandler(_context).Handle(command, new CancellationToken());

            Assert.AreEqual(TicketStatus.Closed, ticket.TicketStatus);
        }

        [Test]
        public void Throws_onWrongId()
        {
            const int WrongId = -1;
            var command = new CloseTicket{ TicketId = WrongId };
            var handler = new CloseTicketHandler(_context);

            string errorMsg = $"Entity \"Ticket\" ({WrongId}) was not found.";

            var act = Assert.ThrowsAsync<NotFoundException>(async () =>
                                await handler.Handle(command, new CancellationToken()));

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);

        }
    }
}
