using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Queries.TicketDetails
{
    public class TicketEntryDTO : IMapFrom<TrackEntry>
    {       
        public string Content { get; set; }
        public UserDTO Author { get; set; }
    }
}