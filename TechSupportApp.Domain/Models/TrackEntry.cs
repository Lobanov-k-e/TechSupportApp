using TechSupportApp.Domain.Common;

namespace TechSupportApp.Domain.Models
{
    public class TrackEntry : AuditableEntity
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        
    }
}