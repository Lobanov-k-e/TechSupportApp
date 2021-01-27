using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechSupportApp.Domain.Models;
using TechSupportApp.Infrastructure.Persistence;

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
            var users = Enumerable
                .Range(1, 10)
                .Select(i => new User { Name = $"user{i}" })
                .ToList();

            var tickets = Enumerable
                .Range(1, 10)
                .Select(i => Ticket.Create($"body{i}", users[i - 1]))
                .ToList();

            for (int i = 0; i < tickets.Count; i++)
            {
                var ticket = tickets[i];
                ticket.Issue = $"test issue{i}";
                ticket.AddMessage($"test issue{i}", ticket.Issuer);                
            }

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
