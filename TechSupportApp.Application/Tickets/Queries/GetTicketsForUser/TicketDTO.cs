using AutoMapper;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Queries.GetTicketsForUser
{
    public class TicketDTO : IMapFrom<Ticket>
    {

        public int Id { get; set; }       
        public string Issue { get; set; }
        public int TicketStatus { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ticket, TicketDTO>()
                .ForMember(m => m.TicketStatus,
                           options => options.MapFrom(s => (int)s.TicketStatus));                
        }
    }
}