using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TechSupportApp.Application.Common.Exceptions;
using TechSupportApp.Application.Mappings;
using TechSupportApp.Application.Tickets.Queries.TicketDetails;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Tests.Application.Tickets.Queries
{
    [TestFixture]
    class TicketDetailsTests
    {
        private ApplicationContext _context;
        public TicketDetailsTests()
        {
            _context = DBContextFactory.Create();
        }

        [Test]
        public async Task CanGetTicket()
        {
            var ticket = await _context.Tickets.FirstAsync();

            var query = new TicketDetails() { Id = ticket.Id };
            var handler = new TicketDetailsHandler(_context, GetMapper());

            var result = await handler.Handle(query, new System.Threading.CancellationToken());

            Assert.IsNotNull(result as TicketDetailsVm);

        }

        [Test]
        public async Task GetsCorrectDetails()
        {
            //лучше создать и добавить новый тикет
            var expected = await _context.Tickets.Select(t => new
            {
                t.Id,
                t.Issue,
                t.TicketStatus
            })
           .FirstAsync();

            var query = new TicketDetails() { Id = expected.Id };
            var handler = new TicketDetailsHandler(_context, GetMapper());

            var result = await handler.Handle(query, new System.Threading.CancellationToken());

            Assert.AreEqual(expected.Id, result.Ticket.Id);
            Assert.AreEqual(expected.Issue, result.Ticket.Issue);
            Assert.AreEqual((int)expected.TicketStatus, result.Ticket.TicketStatus);
        }

        [Test]
        public async Task GetsCorrectIssuer()
        {

            var expected = await _context.Tickets.Select(t => new
            {
                t.Id,
                t.Issuer
            })
           .FirstAsync();

            var query = new TicketDetails() { Id = expected.Id };
            var handler = new TicketDetailsHandler(_context, GetMapper());

            var result = await handler.Handle(query, new System.Threading.CancellationToken());

            Assert.AreEqual(expected.Issuer.Id, result.Ticket.Issuer.Id);
            Assert.AreEqual(expected.Issuer.Name, result.Ticket.Issuer.Name);
        }

        [Test]
        public async Task GetsCorrectTrack()
        {
            //лучше создать и добавить новый тикет
            var expected = await _context.Tickets.Select(t => new
            {
                t.Id,
                t.Track
            })
           .FirstAsync();

            var query = new TicketDetails() { Id = expected.Id };
            var handler = new TicketDetailsHandler(_context, GetMapper());

            var result = (await handler.Handle(query, new System.Threading.CancellationToken()))
                .Ticket
                .Entries;

            CollectionAssert.IsNotEmpty(result);
            
            Assert.AreEqual(expected.Track.Count(), result.Count());            
            
            CollectionAssert.AreEqual(expected.Track, result, new TrackComparer());
        }

        [Test]
        public void Throws_onWrongIdn()
        {
            const int InvalidId = -1;
            string errorMsg = $"Entity \"Ticket\" ({InvalidId}) was not found.";
            var query = new TicketDetails() { Id = InvalidId };
            var handler = new TicketDetailsHandler(_context, GetMapper());

            var act = Assert.ThrowsAsync<NotFoundException>(async () =>
                            await handler.Handle(query, new System.Threading.CancellationToken()));

            StringAssert.AreEqualIgnoringCase(errorMsg, act.Message);
        }


        //create base class for tests
        private IMapper GetMapper()
        {

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return configurationProvider.CreateMapper();
        }

        private class TrackComparer : IComparer
        {
            public int Compare([AllowNull] object x, [AllowNull] object y)
            {
                var t1 = x as dynamic;
                var t2 = y as dynamic;

                if (t1.Content == t2.Content 
                    && t1.Author.Name == t2.Author.Name 
                    && t1.Author.Id == t2.Author.Id)
                {
                    return 0;
                }
                else return -1;
            }            
        }
    }
}
