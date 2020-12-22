using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TechSupportApp.Domain.Enums;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Tests.Domain
{
    [TestFixture]
    class TicketTests
    {
        [Test]
        public void CreateTicket_CreatesTicket()
        {
            const string ExpectedIssuer = "TestIssuer";
            const string ExpectedIssue = "TestIssue";

            var ticket = Ticket.Create(ExpectedIssue, ExpectedIssuer);

            Assert.AreEqual(ExpectedIssuer, ticket.Issuer);
            Assert.AreEqual(ExpectedIssue, ticket.Issue);
            Assert.AreEqual(TicketStatus.Open, ticket.TicketStatus);
        }

        [Test]
        public void Close_ClosesTicket()
        {
            var ticket = GetTicket();

            ticket.Close();

            Assert.AreEqual(TicketStatus.Closed, ticket.TicketStatus);
        }

        [Test]
        public void AddMessage_canAddMessage()
        {
            var ticket = GetTicket();
            const string ExpectedContent = "test";
            bool act = ticket.AddMessage(ExpectedContent);

            Assert.IsTrue(act);
            Assert.NotNull(ticket.Track.SingleOrDefault(t => t.Content == ExpectedContent));

        }

        [Test]
        public void AddMessage_notAddToClosedTicket()
        {
            var ticket = GetTicket();
            ticket.TicketStatus = TicketStatus.Closed;

            bool act = ticket.AddMessage("test");

            Assert.IsFalse(act);            
        }          

        private static Ticket GetTicket(string ExpectedIssuer = " ", string ExpectedIssue = " ")
        {
            return new Ticket()
            {
                Issuer = ExpectedIssuer,
                TicketStatus = TicketStatus.Open,
                Track = new List<TrackEntry>()
                {
                    new TrackEntry() { 
                        Id = 1,
                        Content = ExpectedIssue}
                }
            };
        }
    }
}
