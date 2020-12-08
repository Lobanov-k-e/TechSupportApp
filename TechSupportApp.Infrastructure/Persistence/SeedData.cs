using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Infrastructure.Persistence
{
    class SeedData
    {
        public static void Initialize(ApplicationContext context)
        {
            var tickets = Enumerable.Range(1, 10).Select(i => Ticket.Create($"body{i}", $"issuer{i}")).ToList();
            context.Tickets.AddRange(tickets);
            context.SaveChanges();
        }
    }
}
