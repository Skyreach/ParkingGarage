using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingGarage.Models;
using ParkingGarage.Repositories;
using ParkingGarage.Services;

namespace ParkingGarage.Controllers {
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : Controller {
        private readonly ILocationService _locationService;
        private readonly ITicketService _ticketService;

        public TicketsController(ILocationService locationService, ITicketService ticketService) {
            _ticketService = ticketService;
            _locationService = locationService;
        }

        // GET: api/Tickets
        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> Index() {

            //todo: move cosmos calls into TicketService..
            var tickets = await _ticketService.GetAsync();
            return View(tickets);
        }

        // GET: api/Tickets/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult> Get(string id) {
            var item = await _ticketService.GetAsync(id);
            return View(item);
        }

        // POST: api/Tickets
        [HttpPost]
        //[ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Post([FromBody] string licensePlate) {
            var ticket = new Ticket() {
                LicensePlate = licensePlate,

                /*Todo: Rate, intervals, and increase% should all be stored on the ticket, 
                * to ensure that the customer pays what they're expecting when they receive the ticket */

                //Rate = _locationService.GetRate(),
                //Intervals = _locationService.GetIntervalDurations(),
                //IncreasePercentage = _locationService.GetRateIncreasePercentage()
            };

            if (ModelState.IsValid) {
                await _ticketService.Create(ticket);
                return RedirectToAction("index");
            }

            return View(ticket);
        }
    }
}
