using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application
{
    interface IAppContext
    {
        DbSet<Ticket> Tickets { get; set; }
        Task SaveChangesAsync(CancellationToken cancellationToken);       
    }
}
