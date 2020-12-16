using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechSupportApp.Domain.Enums;
using TechSupportApp.Application.Tickets.Common;

namespace TechSupportApp.Application.Tickets.Queries.GetAllTickets
{
    public class GetAllTicketsVm
    {
        public IEnumerable<TicketDTO> Tickets { get; set; }

        public IEnumerable<TicketStatusDTO> TicketStatusList = TicketStatusHelper.GetTicketStatusDTOs();       
    }
}
