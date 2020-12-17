﻿using System;
using System.Collections.Generic;
using System.Text;
using TechSupportApp.Domain.Common;
using TechSupportApp.Domain.Enums;

namespace TechSupportApp.Domain.Models
{
    public class Ticket : AuditableEntity
    {
        public int Id { get; set; }
        //to-do issue enity 
        public ICollection<TicketEntry> Entries { get; set; }
        //to-do user entity
        public string Issuer { get; set; }

        public TicketStatus TicketStatus { get; set; }

        internal Ticket()
        {
            TicketStatus = TicketStatus.Open;
        }        

        public void Close()
        {
            TicketStatus = TicketStatus.Closed;
        }
       

        public TicketStatus MoveToNextStatus()
        {
            if (TicketStatus != TicketStatus.Closed)
            {
                TicketStatus += 1;
            }
            return TicketStatus;         
            
        }
        public bool AddIssue(string issue)
        {
            if (TicketStatus == TicketStatus.InWork)
            {
                Entries.Add(new TicketEntry
                {
                    Issue = issue
                });
                return true;
            }

            return false;
        }

        public static Ticket Create(string issue, string issuer)
        {
            return new Ticket()
            {
                Issuer = issuer,
                Entries = new List<TicketEntry>()
                {
                    new TicketEntry() { Issue = issue }
                }
            };
        }
    }
}
