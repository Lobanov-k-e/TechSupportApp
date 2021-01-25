using System.Collections.Generic;

namespace TechSupportApp.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<TrackEntry> TrackEntries { get; set; }
    }
}