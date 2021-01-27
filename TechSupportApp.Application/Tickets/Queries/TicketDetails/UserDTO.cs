using System;
using System.Collections.Generic;
using System.Text;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Queries.TicketDetails
{
    public class UserDTO : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
