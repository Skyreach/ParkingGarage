using Microsoft.Extensions.Options;
using ParkingGarage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarage.Services {
    public class LocationService : ILocationService {

        private readonly LocationSettings _locationSettings;

        public LocationService(IOptions<LocationSettings> locationSettings) {
            _locationSettings = locationSettings.Value;
        }

        public int[] GetIntervalDurations() {
            return _locationSettings.RateDurations.Split(',').Select(int.Parse).ToArray();
        }

        public double GetRate() {
            return double.Parse(_locationSettings.BaseRate);
        }

        public double GetRateIncreasePercentage() {
            return double.Parse(_locationSettings.RateIncreasePercent);
        }
    }
}
