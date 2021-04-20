using System;
using TechSupportApp.Application.Interfaces;

namespace TechSupportApp.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
