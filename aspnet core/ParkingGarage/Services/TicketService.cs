using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using ParkingGarage.Models;
using ParkingGarage.Repositories;

namespace ParkingGarage.Services {
    public class TicketService : ITicketService {

        private readonly ICosmosDBRepository<Ticket> _cosmosDBRepository;
        private readonly ILocationService _locationService;

        public TicketService(ICosmosDBRepository<Ticket> cosmosDBRepository, ILocationService locationService) {
            _cosmosDBRepository = cosmosDBRepository;
            _locationService = locationService;
        }

        public double GetAmountOwing(Ticket ticket) {
            var baseRate = _locationService.GetRate();
            var durations = _locationService.GetIntervalDurations();
            var increasePercentage = _locationService.GetRateIncreasePercentage();

            var timeSpan = (DateTime.Now - ticket.TimeIssued).TotalMinutes;

            for (var index = 0; index < durations.Length; index++) {
                if (timeSpan > durations[index])
                    baseRate *= increasePercentage;
                else break;
            }

            return baseRate;
        }

        public Task<Document> Create(Ticket ticket) {
            return _cosmosDBRepository.CreateItemAsync(ticket);
        }

        public Task<Document> Update(Ticket ticket) {
            return _cosmosDBRepository.UpdateItemAsync(ticket.Id.ToString(), ticket);
        }

        public Task<IEnumerable<Ticket>> GetAsync(bool isActive = true) {
            return _cosmosDBRepository.GetItemsAsync(ticket => ticket.IsActive == isActive);
        }

        public Task<IEnumerable<Ticket>> GetAll() {
            return _cosmosDBRepository.GetItemsAsync(ticket => ticket.Id != new Guid().ToString());
        }

        public async Task<Ticket> GetAsync(string id, bool isActive = true) {
            var ticket = await _cosmosDBRepository.GetItemAsync(id);
            if (ticket == null) return null;

            ticket.AmountOwing = GetAmountOwing(ticket);

            return ticket;
        }

        public async Task<Ticket> GetExistingAsync(string licensePlate) {
            var ticketResult = await _cosmosDBRepository.GetItemsAsync(doc => doc.LicensePlate == licensePlate && doc.IsActive);
            var ticket = ticketResult.FirstOrDefault();

            if (ticket != null) {
                ticket.AmountOwing = GetAmountOwing(ticket);
            }

            return ticket;
        }
    }
}
