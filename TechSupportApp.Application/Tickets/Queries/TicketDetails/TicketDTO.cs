using AutoMapper;
using System.Collections;
using System.Collections.Generic;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Application.Tickets.Queries.TicketDetails
{
    public class TicketDTO : IMapFrom<Ticket>
    {

        public int Id { get; set; }
        public string Issuer { get; set; }      

        public IEnumerable<TicketEntryDTO> Entries { get; set; }
        public int TicketStatus { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ticket, TicketDTO>()
                .ForMember(m => m.TicketStatus,
                           options => options.MapFrom(s => (int)s.TicketStatus))
                .ForMember(m => m.Entries,
                           options => options.MapFrom(s=>s.Track) );
            
        }

    }
}
