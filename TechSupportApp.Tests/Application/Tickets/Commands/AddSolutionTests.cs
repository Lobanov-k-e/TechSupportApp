using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Application.Tickets.Commands;
using System.Threading;
using TechSupportApp.Application.Common.Exceptions;

namespace TechSupportApp.Tests.Application.Tickets.Commands
{
    [TestFixture]
    class AddSolutionTests
    {
        IAppContext _context;
        public AddSolutionTests()
        {
            _context = DBContextFactory.Create();
        }

        [Test]
        public async Task CanAddSolution()
        {
            var ticket = await _context.Tickets.FirstAsync();
            var issue = ticket.Entries.First();
            const string expected = "test solution";

            var command = new AddSolution { TicketId = ticket.Id, IssueId = issue.Id, Solution = expected };

            await new AddSolutionHandler(_context).Handle(command, new CancellationToken());

            var actual = (await _context.Tickets.FindAsync(ticket.Id)).Entries.SingleOrDefault(e => e.Id == command.IssueId);

            StringAssert.AreEqualIgnoringCase(expected, actual.Solution);            
        }

        [Test]
        public void Throws_onWrongTicketId()
        {
            const int WrongId = -1;
            var command = new AddSolution { TicketId = WrongId, IssueId = 1, Solution = " " };
            var handler = new AddSolutionHandler(_context);

            string errorMsg = $"Entity \"Ticket\" ({WrongId}) was not found.";

            var act = Assert.ThrowsAsync<NotFoundException>(async () => 
                                await handler.Handle(command, new CancellationToken()) );

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }       
    }
}
