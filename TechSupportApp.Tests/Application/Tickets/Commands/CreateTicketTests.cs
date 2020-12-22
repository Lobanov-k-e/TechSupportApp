using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Tickets.Commands;
using TechSupportApp.Application.Tickets.Commands.CreateTicket;
using TechSupportApp.Infrastructure;

namespace TechSupportApp.Tests.Application.Tickets.Commands
{
    [TestFixture]
    class CreateTicketTests
    {
        private ApplicationContext _contex;

        public CreateTicketTests()
        {
            _contex = DBContextFactory.Create();
        }

        [Test]
        public async Task CreateTicket_can_Create()
        {
            var command = CreateCommand();
            var handler = new CreateTicketRequestHandler(_contex);

            int ticketCount = await _contex.Tickets.CountAsync();

            var result = await handler.Handle(command, new CancellationToken());

            Assert.IsNotNull(await _contex.Tickets.FindAsync(result));
            Assert.AreEqual(ticketCount + 1, await _contex.Tickets.CountAsync());
        }

        [Test]
        public async Task CreateTicket_creates_CorrectTicket()
        {
            var command = CreateCommand();
            var handler = new CreateTicketRequestHandler(_contex);           

            int ticketId = await handler.Handle(command, new CancellationToken() );

            var result = await _contex.Tickets.FindAsync(ticketId);

            StringAssert.AreEqualIgnoringCase(command.Issue, result.Issue);
            StringAssert.AreEqualIgnoringCase(command.Issuer, result.Issuer);
        }

        private static CreateTicket CreateCommand()
        {
           return new CreateTicket() { Issue = "testContent", Issuer = "testIssuer" };
        }
    }
}
