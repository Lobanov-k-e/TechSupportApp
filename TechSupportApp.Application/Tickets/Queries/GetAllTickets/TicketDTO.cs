using AutoMapper;
using System.Linq;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Queries.GetAllTickets
{
    public class TicketDTO : IMapFrom<Ticket>
    {
       
        public int Id { get; set; }        
        public string Issuer { get; set; }
        public string Issue { get; set; }
        public int TicketStatus { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ticket, TicketDTO>()
                .ForMember(m => m.TicketStatus,
                           options => options.MapFrom(s => (int)s.TicketStatus))
                .ForMember(m => m.Issuer,
                            options => options.MapFrom(s => s.Issuer.Name) );              
        }        
    }
}
