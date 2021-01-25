using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
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
        private UserServiceFactory _userServiceFactory;

        public CreateTicketTests()
        {
            _context = DBContextFactory.Create();
            _userServiceFactory = new UserServiceFactory();
        }

        [Test]
        public async Task CreateTicket_can_Create()
        {
            var userService = _userServiceFactory.Create();
            var handler = GetHandler(userService);

            int ticketCount = await _context.Tickets.CountAsync();

            var user = await _context.Users.FirstAsync();
            var command = GetCommand(It.IsAny<string>(), GetUserIdentity(user));          

            var result = await handler.Handle(command, new CancellationToken());

            Assert.IsNotNull(await _context.Tickets.FindAsync(result));
            Assert.AreEqual(ticketCount + 1, await _context.Tickets.CountAsync());
        }       

        [Test]
        public async Task CreateTicket_creates_CorrectTicket()
        {
            var userService = _userServiceFactory.Create();
            var handler = GetHandler(userService);

            var user = await _context.Users.FirstAsync();
            var command = GetCommand("testContent", GetUserIdentity(user));           

            int ticketId = await handler.Handle(command, new CancellationToken() );
            var result = await _context.Tickets.FindAsync(ticketId);

            StringAssert.AreEqualIgnoringCase(command.Issue, result.Issue);
            StringAssert.AreEqualIgnoringCase(command.UserId, GetUserIdentity(result.Issuer) );
        }
        [Test]
        public async Task CreateTicket_passesCorrectIdentity()
        {
            var userService = _userServiceFactory.Create();
            var handler = GetHandler(userService);

            var user = await _context.Users.FirstAsync();                    
            string userIdentity = GetUserIdentity(user);
            
            var command = GetCommand(It.IsAny<string>(), userIdentity);           

            await handler.Handle(command, new CancellationToken());

            Assert.AreEqual(userIdentity, userService.DomainId);
        }
        [Test]
        public void Throws_userNotFound()
        {                          
            var userService = _userServiceFactory.Create();
            var handler = GetHandler(userService);

            const string WrongIdentity = "-1";  
            var command = GetCommand(It.IsAny<string>(), WrongIdentity);

            string errorMsg = $"Entity \"User\" ({WrongIdentity}) was not found.";

            var act = Assert.ThrowsAsync<NotFoundException>(async () =>
                                await handler.Handle(command, new CancellationToken()));

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }
        [Test]
        public void Throws_identityNotFound()
        {                        
            var userService = _userServiceFactory.Create();
            userService.ShouldFailNextCall = true;
            
            var handler = GetHandler(userService);

            var command = GetCommand(It.IsAny<string>(), It.IsAny<string>());

            var act = Assert.ThrowsAsync<ApplicationException>(async () =>
                                await handler.Handle(command, new CancellationToken()));            
        }

        private static CreateTicket GetCommand(string content, string userIdentity)
        {
           return new CreateTicket() { Issue = content, UserId = userIdentity };
        }
        private CreateTicketRequestHandler GetHandler(IIdentityService service)
        {

            return new CreateTicketRequestHandler(_context, service);
        }
        /// <summary>
        /// converts user domain id to identity id
        /// </summary>
        /// <param name="user"></param>
        /// <returns>string representation of user id</returns>
        private static string GetUserIdentity(User user)
        {
            return user.Id.ToString();
        }
    }
}
