using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechSupportApp.Domain.Models;
using TechSupportApp.Infrastructure;

namespace TechSupportApp.Tests.Application
{
    class DBContextFactory
    {
        public static ApplicationContext Create(bool asNoTracking = false)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            if (asNoTracking)
            {
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }


            var context = new ApplicationContext(optionsBuilder.Options);
            context.Database.EnsureCreated();
            SeedData(context);
            if (asNoTracking)
            {
                foreach (var entry in context.ChangeTracker.Entries())
                {
                    entry.State = EntityState.Detached;
                }
            }

            return context;
        }

        private static void SeedData(ApplicationContext context)
        {
            var tickets = Enumerable.Range(1, 10).Select(i=> Ticket.Create($"body{i}", $"issuer{i}")).ToList();
            context.Tickets.AddRange(tickets);
            context.SaveChanges();
        }

        public static void Destroy(ApplicationContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
