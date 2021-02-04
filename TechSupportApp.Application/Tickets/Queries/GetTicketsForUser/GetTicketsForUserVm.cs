using System.Collections.Generic;
using TechSupportApp.Application.Tickets.Common;

namespace TechSupportApp.Application.Tickets.Queries.GetTicketsForUser
{
    public class GetTicketsForUserVm
    {
        public IEnumerable<TicketDTO> Tickets { get; set; }

        public IEnumerable<TicketStatusDTO> TicketStatusList = TicketStatusHelper.GetTicketStatusDTOs();
    }
}