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
            const string ExpectedIssue = "TestIssue";
            var ExpectedIssuer = new User();

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
            const string ExpectedUserName = "TestUser";
            bool act = ticket.AddMessage(ExpectedContent, new User() { Name = ExpectedUserName });           

            Assert.IsTrue(act);
            Assert.NotNull(ticket.Track.SingleOrDefault(t => t.Content == ExpectedContent));
            Assert.IsTrue(ticket.Track.SingleOrDefault(t => t.Content == ExpectedContent).Author.Name == ExpectedUserName);

        }

        [Test]
        public void AddMessage_notAddToClosedTicket()
        {
            var ticket = GetTicket();
            ticket.TicketStatus = TicketStatus.Closed;

            bool act = ticket.AddMessage("test", new User());

            Assert.IsFalse(act);            
        }          

        private static Ticket GetTicket(string ExpectedIssuerName = " ", string ExpectedIssue = " ")
        {
            return new Ticket()
            {
                Issuer = new User { Name = ExpectedIssuerName},
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
