using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Application.Tickets.Queries.GetTicketsForUser;
using TechSupportApp.Infrastructure.Persistence;
using TechSupportApp.Tests.Application.Common;
using System.Threading;
using System.Linq;
using Moq;

namespace TechSupportApp.Tests.Application.Tickets.Queries
{
    [TestFixture]
    class GetTicketsForUserTests
    {
        private ApplicationContext _context;
        private IdentityServiceFactory _identityServiceFactory;
        
        public GetTicketsForUserTests()
        {
            _context = DBContextFactory.Create();
            _identityServiceFactory = new IdentityServiceFactory();
        }

        [Test]
        public async Task CanGetTickets()
        {
            var user = await _context.Users.FirstAsync();
            var expectedCount = user.Tickets.Count;

            var command = GetCommand(user.Id.ToString() );

            var handler = GetHandler(_context, _identityServiceFactory.Create() );

            var result = await handler.Handle(command, new CancellationToken() );

            CollectionAssert.IsNotEmpty(result.Tickets);
            Assert.AreEqual(expectedCount, result.Tickets.Count());
        }

        [Test]
        public void Throws_IdentityNotFound()
        {           
            FakeIdentityService identityService = _identityServiceFactory.Create();
            identityService.ShouldFailNextCall = true;

            var command = GetCommand(It.IsAny<string>());
            var handler = GetHandler(_context, identityService);

            Assert.ThrowsAsync<ApplicationException>(async () => await handler.Handle(command, new CancellationToken()));
        }

        private GetTicketsForUser GetCommand(string identity)
        {
            return new GetTicketsForUser
            {
                CurrentUserIdentity = identity
            };
        }

        private GetTicketsForUserHandler GetHandler(IAppContext context, IIdentityService identityService)
        {
            return new GetTicketsForUserHandler(MapperFactory.GetMapper(), context, identityService);
        }
      
    }
}
