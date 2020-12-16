using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechSupportApp.Domain.Enums;

namespace TechSupportApp.Application.Tickets.Common
{
    static class TicketStatusHelper
    {
        internal static IEnumerable<TicketStatusDTO> GetTicketStatusDTOs()
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
