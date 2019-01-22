using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingGarage.Models;
using ParkingGarage.Services;

namespace ParkingGarage.Controllers
{
    [Route("payments")]
    [ApiController]
    public class TicketPaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ITicketService _ticketService;
        
        public TicketPaymentsController(IPaymentService paymentService, ITicketService ticketService) {
            _paymentService = paymentService;
            _ticketService = ticketService;
        }

        [HttpPost]
        [Route("{ticketId}")]
        public async Task<string> Post(string ticketId, [FromBody] string creditNumber)
        {
            if (!_paymentService.IsValidCard(creditNumber)) {
                return "Failed to pay for ticket";
            }

            Ticket ticket = null;
            if (ModelState.IsValid) {
                var ticketResult = await _ticketService.GetAsync(ticketId);

                if (ticketResult != null) {
                    ticket = await _paymentService.PayTicket(ticketResult);
                } else {
                    return "Failed to pay for ticket, no tickets found!";
                }
            }

            return $"Ticket {ticket.Id} has been paid with credit card {creditNumber} for amount ${ticket.AmountOwing.ToString("F2")}";
        }
    }
}
