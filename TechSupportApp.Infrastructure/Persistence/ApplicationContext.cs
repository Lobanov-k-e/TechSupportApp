﻿using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Infrastructure
{
    public class ApplicationContext : DbContext, IAppContext
    {
        public ApplicationContext([NotNull] DbContextOptions options) 
            : base(options)
        {
        }


        public DbSet<Ticket> Tickets { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken() )
        {
            return await base.SaveChangesAsync(cancellationToken);           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}