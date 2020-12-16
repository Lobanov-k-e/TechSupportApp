using System.Collections.Generic;
using TechSupportApp.Application.Tickets.Common;

namespace TechSupportApp.Application.Tickets.Queries.TicketDetails
{
    public class TicketDetailsVm
    {
        public TicketDTO Ticket { get; set; }

        public IEnumerable<TicketStatusDTO> TicketStatusList = TicketStatusHelper.GetTicketStatusDTOs();
    }
}