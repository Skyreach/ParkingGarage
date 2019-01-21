using ParkingGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarage.Services
{
    public interface IPaymentService {
        bool IsValidCard(string creditNumber);
        Task<Ticket> PayTicket(Ticket ticket);
    }
}
