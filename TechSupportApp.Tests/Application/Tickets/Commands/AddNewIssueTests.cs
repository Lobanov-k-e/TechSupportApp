using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Tickets.Commands.AddIssue;
using TechSupportApp.Domain.Enums;
using TechSupportApp.Infrastructure;

namespace TechSupportApp.Tests.Application.Tickets.Commands
{
    [TestFixture]
    class AddNewIssueTests
    {
        private ApplicationContext _context;

        public AddNewIssueTests()
        {
            _context = DBContextFactory.Create();
        }
        [Test]
        public async Task CanAdd()
        {
            var ticket = await _context.Tickets.FindAsync(1);

            ticket.TicketStatus = TicketStatus.InWork;

            var command = new AddNewIssue
            {
                TicketId = ticket.Id,
                Issue = "Test"
            };

            int expectedNumberOfIssues = ticket.Entries.Count + 1;

            await new AddNewIssueRequestHandler(_context).Handle(command, new CancellationToken());

            var actual = await _context.Tickets.FindAsync(1);

            Assert.AreEqual(expectedNumberOfIssues, actual.Entries.Count);

        }
        [Test]
        public async Task NotAdd_if_wrongTicketStatus()
        {
            var ticket = await _context.Tickets.FindAsync(1);

            ticket.TicketStatus = TicketStatus.Open;

            var command = new AddNewIssue
            {
                TicketId = ticket.Id,
                Issue = "Test"
            };

            int expectedNumberOfIssues = ticket.Entries.Count;

            await new AddNewIssueRequestHandler(_context).Handle(command, new CancellationToken());

            ticket.TicketStatus = TicketStatus.Closed;
            await new AddNewIssueRequestHandler(_context).Handle(command, new CancellationToken());

            var actual = await _context.Tickets.FindAsync(1);

            Assert.AreEqual(expectedNumberOfIssues, actual.Entries.Count);
        }

        [Test]
        public void Throws_onWrongId()
        {
            const int InvalidId = -1;
            string errorMsg = $"Entity \"Ticket\" ({InvalidId}) was not found.";
            var command = new AddNewIssue
            {
                TicketId = InvalidId,
                Issue = "Test"
            };
            var handler = new AddNewIssueRequestHandler(_context);

            var act = Assert.ThrowsAsync<NotFoundException>(async () => 
                            await handler.Handle(command, new CancellationToken()) );
            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }
    }
}
