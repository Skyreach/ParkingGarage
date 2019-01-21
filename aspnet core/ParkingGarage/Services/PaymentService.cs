using ParkingGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarage.Services
{
    public class PaymentService : IPaymentService {

        private readonly ITicketService _ticketService;

        public PaymentService(ITicketService ticketService) {
            _ticketService = ticketService;
        }

        public bool IsValidCard(string creditNumber) {
            //amazing validation, go!
            return !string.IsNullOrWhiteSpace(creditNumber);
        }

        public Task<Ticket> PayTicket(Ticket ticket) {

            var amountOwing = _ticketService.GetAmountOwing(ticket);

            ticket.isPaid = true;
            ticket.IsActive = false;
            ticket.AmountOwing = amountOwing;

            _ticketService.Update(ticket);

            return Task.FromResult(ticket);
        }
    }
}
