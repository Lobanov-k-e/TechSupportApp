using System.Collections.Generic;
using System.Linq;
using TechSupportApp.Domain.Common;
using TechSupportApp.Domain.Enums;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TechSupportApp.Tests")]

namespace TechSupportApp.Domain.Models
{
    //coздать интерфейс сервиса, который будет производить доменный объект пользователя на основе пользователя identity.
    //реализация сервиса - в аппликейшне. пользователя будем получать не из AppContext, а из IdentityContext (один или разные контексты?)
    public class Ticket : AuditableEntity
    {
        public int Id { get; set; }        
        public ICollection<TrackEntry> Track { get; set; }        
        public User Issuer { get; set; }
        public string Issue { get; set; }              
        public TicketStatus TicketStatus { get; set; }
        internal Ticket()
        {
            TicketStatus = TicketStatus.Open;
            Track = new List<TrackEntry>();
        }        
        public void Close()
        {
            TicketStatus = TicketStatus.Closed;
        }                  
        public bool AddMessage(string content, User author)
        {
            if (TicketStatus == TicketStatus.Closed)
                return false;

            Track.Add(new TrackEntry(){
                Content = content,
                Author = author
            });            
            
            return true;           
        }
        public static Ticket Create(string issue, User issuer)
        {
            return new Ticket()
            {
                Issuer = issuer,
                Issue = issue
            };
        }
    }
}
