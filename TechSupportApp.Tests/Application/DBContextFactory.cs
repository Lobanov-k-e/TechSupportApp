using System;
using System.Collections.Generic;
using System.Text;

namespace TechSupportApp.Tests.Application
{
    class DBContextFactory
    {
        //public static ApplicationContext Create(bool asNoTracking = false)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString());
        //    if (asNoTracking)
        //    {
        //        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        //    }


        //    var context = new ApplicationContext(optionsBuilder.Options);
        //    context.Database.EnsureCreated();
        //    SeedData(context);
        //    if (asNoTracking)
        //    {
        //        foreach (var entry in context.ChangeTracker.Entries())
        //        {
        //            entry.State = EntityState.Detached;
        //        }
        //    }

        //    return context;
        //}

        //public static void Destroy(ApplicationContext context)
        //{
        //    context.Database.EnsureDeleted();
        //    context.Dispose();
        //}
    }
}
