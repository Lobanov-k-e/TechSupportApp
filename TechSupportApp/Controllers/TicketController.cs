using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechSupportApp.Application.Common;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Application.Tickets.Commands.CreateTicket;
using TechSupportApp.Application.Tickets.Queries.GetTicketsForUser;
using TechSupportApp.Application.Tickets.Queries.TicketDetails;

namespace TechSupportApp.WebApi.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {

        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public TicketController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(paramName: nameof(mediator), message: "mediator should not be null");
            _currentUserService = currentUserService ?? 
                throw new ArgumentNullException(nameof(currentUserService), message: "current user service should not be null");
        }
        public async Task<IActionResult> Index() 
        {
            var userId = _currentUserService.UserId;

            var tickets = await _mediator.Send(new GetTicketsForUser() { CurrentUserIdentity = userId });
            return View(tickets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicket request)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(request);
                return Redirect(nameof(Index));
            }
            return View(request);
        }

        public async Task<IActionResult> Details(TicketDetails request)
        {
            return View(await _mediator.Send(request));
        }


        public async Task<IActionResult> SeedData()
        {
            await _mediator.Send(new SeedDataCommand());
            return RedirectToAction(nameof(Index));
        }
    }
}