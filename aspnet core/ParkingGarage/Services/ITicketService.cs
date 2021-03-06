﻿using Microsoft.Azure.Documents;
using ParkingGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarage.Services {
    public interface ITicketService {
        double GetAmountOwing(Ticket ticket);

        Task<IEnumerable<Ticket>> GetAsync(bool isActive = true);

        Task<IEnumerable<Ticket>> GetAll();

        Task<Ticket> GetAsync(string id, bool isActive = true);

        Task<Document> Create(Ticket ticket);

        Task<Document> Update(Ticket ticket);

        Task<Ticket> GetExistingAsync(string licensePlate);
    }
}
