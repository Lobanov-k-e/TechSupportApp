using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Application.Tickets.Commands.CreateTicket;
using TechSupportApp.Domain.Models;
using TechSupportApp.Infrastructure.Persistence;
using TechSupportApp.Tests.Application.Common;

namespace TechSupportApp.Tests.Application.Tickets.Commands
{
    [TestFixture]
    class CreateTicketTests
    {
        private ApplicationContext _context;
        private IIdentityService _userService;

        public CreateTicketTests()
        {
            _context = DBContextFactory.Create();
            _userService = UserServiceFactory.Create(_context);
        }

        [Test]
        public async Task CreateTicket_can_Create()
        {
            var user = await _context.Users.FirstAsync();

            var command = GetCommand(It.IsAny<string>(), GetUserIdentity(user));

            var handler = GetHandler();

            int ticketCount = await _context.Tickets.CountAsync();

            var result = await handler.Handle(command, new CancellationToken());

            Assert.IsNotNull(await _context.Tickets.FindAsync(result));
            Assert.AreEqual(ticketCount + 1, await _context.Tickets.CountAsync());
        }       

        [Test]
        public async Task CreateTicket_creates_CorrectTicket()
        {
            var user = await _context.Users.FirstAsync();
            var command = GetCommand("testContent", GetUserIdentity(user));         

            var handler = new CreateTicketRequestHandler(_context, _userService);

            int ticketId = await handler.Handle(command, new CancellationToken() );

            var result = await _context.Tickets.FindAsync(ticketId);

            StringAssert.AreEqualIgnoringCase(command.Issue, result.Issue);
            StringAssert.AreEqualIgnoringCase(command.userId, GetUserIdentity(result.Issuer) );
        }

        [Test]
        public void Throws_onWrongUserIdentity()
        {            
            const string WrongIdentity = "-1";
            var command = GetCommand(It.IsAny<string>(), WrongIdentity);
            var handler = GetHandler();

            string errorMsg = $"Entity \"User\" ({WrongIdentity}) was not found.";

            var act = Assert.ThrowsAsync<NotFoundException>(async () =>
                                await handler.Handle(command, new CancellationToken()));

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }

        private static CreateTicket GetCommand(string content, string userIdentity)
        {
           return new CreateTicket() { Issue = content, userId = userIdentity };
        }
        private CreateTicketRequestHandler GetHandler()
        {
            return new CreateTicketRequestHandler(_context, _userService);
        }
        private static string GetUserIdentity(User user)
        {
            return user.Id.ToString();
        }
    }
}
