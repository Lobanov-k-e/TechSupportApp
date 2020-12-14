using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechSupportApp.Application.Common;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Application.Tickets.Commands.CreateTicket;
using TechSupportApp.Application.Tickets.Queries.GetAllTickets;

namespace TechSupportApp.WebApi.Controllers
{
    public class TicketController : Controller
    {

        private readonly IMediator _mediator;

        public TicketController(IAppContext appContext, IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(paramName: nameof(mediator), message: "mediator should not be null");
        }
        public async Task<IActionResult> Index()
        {

            var tickets = await _mediator.Send(new GetAllTickets());
            return View(tickets);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicket command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return Redirect(nameof(Index));
            }
            return View(command);
        }     

        public async Task<IActionResult> SeddData()
        {
            await _mediator.Send(new SeedDataCommand());
            return RedirectToAction(nameof(Index));
        }
    }
}