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
    class AddMessageTests
    {
        IAppContext _context;
        public AddMessageTests()
        {
            _context = DBContextFactory.Create();
        }

        [Test]
        public async Task CanAddMessage()
        {
            var ticket = await _context.Tickets.FirstAsync();          
            const string expected = "test message";

            var command = new AddMessage { TicketId = ticket.Id, Content = expected };

            await new AddMessageHandler(_context).Handle(command, new CancellationToken());

            var actual = (await _context.Tickets.FindAsync(ticket.Id)).Track.SingleOrDefault(t=>t.Content == expected);

            //can assert.notnull
            StringAssert.AreEqualIgnoringCase(expected, actual.Content);            
        }

        [Test]
        public void Throws_onWrongTicketId()
        {
            const int WrongId = -1;
            var command = new AddMessage { TicketId = WrongId, Content = " "};
            var handler = new AddMessageHandler(_context);

            string errorMsg = $"Entity \"Ticket\" ({WrongId}) was not found.";

            var act = Assert.ThrowsAsync<NotFoundException>(async () => 
                                await handler.Handle(command, new CancellationToken()) );

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }       
    }
}
