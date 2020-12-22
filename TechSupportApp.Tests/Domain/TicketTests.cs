using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Assert.AreEqual(ExpectedIssue, ticket.Entries.First().Issue);
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
        public void AddSolution_canAddSolution()
        {
            var ticket = GetTicket();
            const string ExpectedSolution = "test";

            bool act = ticket.AddSolution(1, ExpectedSolution);

            Assert.IsTrue(act);
            Assert.AreEqual(ExpectedSolution, ticket.Entries.First().Solution);
        }

        [Test]
        public void AddSolution_notAddToClosedTicket()
        {
            var ticket = GetTicket();
            ticket.TicketStatus = TicketStatus.Closed;

            bool act = ticket.AddSolution(1, "test");

            Assert.IsFalse(act);
            Assert.IsTrue(string.IsNullOrEmpty(ticket.Entries.First().Solution));
        }

        [Test]
        public void AddSolution_falseIfNoIssueId()
        {
            var ticket = GetTicket();

            bool act = ticket.AddSolution(-1, "test");

            Assert.IsFalse(act);
        }

        [Test]
        public void AddSolution_setCorrectStatus()
        {
            var ticket = GetTicket();

            ticket.AddSolution(1, "test");

            Assert.AreEqual(TicketStatus.InWork, ticket.TicketStatus);
        }

        [Test]
        public void AddNewIssue_canAdd()
        {
            var ticket = GetTicket();
            ticket.TicketStatus = TicketStatus.InWork;
            const string ExpectedIssue = "test";

            bool act = ticket.AddNewIssue(ExpectedIssue);

            Assert.IsTrue(act);
            Assert.IsTrue(ticket.Entries.Count > 1);
            Assert.NotNull(ticket.Entries.SingleOrDefault(e => e.Issue == ExpectedIssue) );
        }

        [TestCase(TicketStatus.Open)]
        [TestCase(TicketStatus.Closed)]
        public void AddNewIssue_notAddIfWrongTicketStatus(TicketStatus status)
        {
            var ticket = GetTicket();
            ticket.TicketStatus = status;

            bool act = ticket.AddNewIssue("test");

            Assert.IsFalse(act);
        }






        private static Ticket GetTicket(string ExpectedIssuer = " ", string ExpectedIssue = " ")
        {
            return new Ticket()
            {
                Issuer = ExpectedIssuer,
                TicketStatus = TicketStatus.Open,
                Entries = new List<TicketEntry>()
                {
                    new TicketEntry() { 
                        Id = 1,
                        Issue = ExpectedIssue}
                }
            };
        }
    }
}
