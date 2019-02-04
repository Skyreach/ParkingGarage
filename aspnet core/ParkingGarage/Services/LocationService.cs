
using Microsoft.Extensions.Options;
using ParkingGarage.Common;
using ParkingGarage.Extensions;
using ParkingGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarage.Services {
    public class LocationService : ILocationService {

        private readonly LocationSettings _locationSettings;
        private int _occupancy;

        public LocationService(IOptions<LocationSettings> locationSettings) {
            _locationSettings = locationSettings.Value;
            _occupancy = _locationSettings.Occupancy;
        }

        public int[] GetIntervalDurations() {
            return _locationSettings.RateDurations.Split(',').Select(int.Parse).ToArray();
        }

        public double GetRate() {
            return _locationSettings.BaseRate;
        }

        public double GetRateIncreasePercentage() {
            return _locationSettings.RateIncreasePercent;
        }

        public int GetOccupancy() {
            return _occupancy;
        }

        public bool TryEnterLocation() {
            if (_occupancy <= 0) return false;

            _occupancy -= 1;
            return true;
        }

        public string ExitLocation(Ticket ticket) {
            if (!ticket.isPaid) return "Failed to exit garage, ticket is not paid!";

            _occupancy += 1;
            // Consider checking against ticketService to see if ticket has been paid.
            return "Exited successfully";
        }
    }
}
