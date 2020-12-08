using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechSupportApp.Domain.Enums;

namespace TechSupportApp.Application.Tickets.Queries.GetAllTickets
{
    public class GetAllTicketsVm
    {
        public IEnumerable<TicketDTO> Tickets { get; set; }

        public IEnumerable<TicketStatusDTO> TicketStatusList = GetTicketStatusList();

        private static IEnumerable<TicketStatusDTO> GetTicketStatusList()
        {
            return Enum.GetValues(typeof(TicketStatus))
                .Cast<TicketStatus>()
                .Select(v => new TicketStatusDTO()
                {
                    Value = (int)v,
                    Name = v.ToString()
                })
                .ToList();
        }
    }
}
