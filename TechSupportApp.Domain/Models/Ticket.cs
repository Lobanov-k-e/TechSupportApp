using System;
using System.Collections.Generic;
using System.Text;
using TechSupportApp.Domain.Common;
using TechSupportApp.Domain.Enums;

namespace TechSupportApp.Domain.Models
{
    public class Ticket : AuditableEntity
    {
        public int Id { get; set; }
        public string Body { get; set; }

        public string Issuer { get; set; }

        public TicketStatus TicketStatus { get; set; }

        internal Ticket()
        {
            TicketStatus = TicketStatus.Open;
        }

        public void Accept()
        {
            TicketStatus = TicketStatus.Accepted;
        }

        public void Close()
        {
            TicketStatus = TicketStatus.Closed;
        }

        public Ticket Create(string body, string issuer)
        {
            return new Ticket()
            { 
                Issuer = issuer,
                Body = body 
            };
        }
    }
}
