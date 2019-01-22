using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingGarage.Extensions;
using ParkingGarage.Models;
using ParkingGarage.Repositories;
using ParkingGarage.Services;

namespace ParkingGarage.Controllers {
    [Route("ticket")]
    [ApiController]
    public class TicketsController : Controller {
        private readonly ILocationService _locationService;
        private readonly ITicketService _ticketService;

        public TicketsController(ILocationService locationService, ITicketService ticketService) {
            _ticketService = ticketService;
            _locationService = locationService;
        }

        [HttpGet]
        [Route("pulse")]
        public string GetPulse() {
            return DateTime.Now.ToString();
        }

        // GET: tickets
        /// <summary>
        /// Retrieves all active tickets
        /// </summary>
        /// <todo>Change result type from IEnumerable<Ticket> to ActionResult</todo>
        /// <returns>All active tickets</returns>
        [HttpGet]
        [ActionName("Index")]
        [Route("")]
        public async Task<IEnumerable<Ticket>> Index() {

            var tickets = await _ticketService.GetAll();
            var sortedTickets = tickets.OrderBy(ticket => ticket.IsActive).ToList();

            return sortedTickets;
        }

        // GET: ticket/5
        /// <summary>
        /// Retrieves a ticket by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <todo>Change result type from Ticket to ActionResult</todo>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<Ticket> Get(string id) {
            return await _ticketService.GetAsync(id);
        }

        // POST: ticket
        /// <summary>
        /// Issues a new ticket if occupancy is available, will re-issue the same ticket if the license
        /// already exists as an active ticket
        /// </summary>
        /// <todo>Change result type from Ticket to ActionResult</todo>
        /// <param name="licensePlate">License Plate of the Vehicle to Register</param>
        /// <returns>The generated or retrieved ticket</returns>
        [HttpPost]
        [Route("")]
        public async Task<Ticket> Post([FromBody] string licensePlate = "No vehicle association") {

            // Validate that the user does not already have a ticket
            var existingTicket = await _ticketService.GetExistingAsync(licensePlate);

            if (existingTicket != null) {
                return existingTicket;
            }

            // Otherwise, issue a new one.
            var hasOccupancy = _locationService.TryEnterLocation();

            Ticket ticket = null;
            if (ModelState.IsValid && hasOccupancy) {
                var baseTicket = new Ticket() {
                    LicensePlate = licensePlate,
                    TimeIssued = DateTime.Now,

                    /*Todo: Ensure that the customer pays what they're expecting when they receive the ticket 
                     * Currently retrieved from location service and not from ticket values */

                    Rate = _locationService.GetRate(),
                    Intervals = _locationService.GetIntervalDurations(),
                    IncreasePercentage = _locationService.GetRateIncreasePercentage()
                };
                var ticketResult = await _ticketService.Create(baseTicket);
                ticket = ticketResult.ToObject<Ticket>();
            }

            return ticket;
        }
    }
}
