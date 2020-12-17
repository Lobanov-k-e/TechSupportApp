using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Queries.TicketDetails
{
    public class TicketEntryDTO : IMapFrom<TicketEntry>
    {
        public string Issue { get; set; }
        public string Solution { get; set; }
    }
}