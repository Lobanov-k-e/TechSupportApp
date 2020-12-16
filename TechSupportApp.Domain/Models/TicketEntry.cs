using TechSupportApp.Domain.Common;

namespace TechSupportApp.Domain.Models
{
    public class TicketEntry : AuditableEntity
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Issue { get; set; }
        public string Solution { get; set; }
    }
}