﻿using Microsoft.EntityFrameworkCore;
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
using TechSupportApp.Tests.Application.Common;
using Moq;

namespace TechSupportApp.Tests.Application.Tickets.Commands
{
    [TestFixture]
    class AddMessageTests
    {
        IAppContext _context;
        IUserService _userService;
        public AddMessageTests()
        {
            _context = DBContextFactory.Create();
            _userService = UserServiceFactory.Create(_context);
        }

        [Test]
        public async Task CanAddMessage()
        {
            var ticket = await _context.Tickets.FirstAsync();          
            const string expected = "test message";
            var user = ticket.Issuer;

            var command = new AddMessage { TicketId = ticket.Id, Content = expected, UserId = user.Id.ToString() };

            await GetHandler(_context, _userService).Handle(command, new CancellationToken());

            var actual = (await _context.Tickets.FindAsync(ticket.Id)).Track.SingleOrDefault(t=>t.Content == expected);

            //can assert.notnull
            StringAssert.AreEqualIgnoringCase(expected, actual.Content);            
        }

        [Test]
        public void Throws_onWrongTicketId()
        {
            const int WrongId = -1;
            var command = new AddMessage { TicketId = WrongId, Content = It.IsAny<string>() };
            var handler = GetHandler(_context, _userService);

            string errorMsg = $"Entity \"Ticket\" ({WrongId}) was not found.";

            var act = Assert.ThrowsAsync<NotFoundException>(async () => 
                                await handler.Handle(command, new CancellationToken()) );

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }

        [Test]
        public async Task Throws_onWrongUserIdentity()
        {
            var ticket = await _context.Tickets.FirstAsync();
            const string WrongIdentity = "-1";
            var command = new AddMessage { TicketId = ticket.Id, Content = It.IsAny<string>(), UserId = WrongIdentity };
            var handler = GetHandler(_context, _userService);

            string errorMsg = $"Entity \"User\" ({WrongIdentity}) was not found.";

            var act = Assert.ThrowsAsync<NotFoundException>(async () =>
                                await handler.Handle(command, new CancellationToken()));

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }

        private AddMessageHandler GetHandler(IAppContext context, IUserService userService)
        {
            return new AddMessageHandler(context, userService);
        }
    }
}
