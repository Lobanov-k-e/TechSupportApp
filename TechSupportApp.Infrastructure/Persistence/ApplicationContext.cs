using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Application;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Common;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Infrastructure.Persistence
{
    public class ApplicationContext : DbContext, IAppContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly ICurrentUserService _currentUserService;

        public ApplicationContext([NotNull] DbContextOptions<ApplicationContext> options, 
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService) 
            : base(options)
        {
            _dateTime = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService) );
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService) );
        }


        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TrackEntry> TicketEntries { get; set; }
        public DbSet<User> Users { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken() )
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
