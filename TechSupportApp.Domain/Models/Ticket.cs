using System.Collections.Generic;
using System.Linq;
using TechSupportApp.Domain.Common;
using TechSupportApp.Domain.Enums;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TechSupportApp.Tests")]

namespace TechSupportApp.Domain.Models
{
    public class Ticket : AuditableEntity
    {
        public int Id { get; set; }        
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
        public bool AddNewIssue(string issue)
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
        public bool AddSolution(int issueId, string solution)
        {
            if (TicketStatus == TicketStatus.Closed)
                return false;

            var entry = Entries.SingleOrDefault(e => e.Id == issueId);

            if (entry is null)
                return false;

            entry.Solution = solution;
           
            TicketStatus = TicketStatus.InWork;            
            return true;           
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
