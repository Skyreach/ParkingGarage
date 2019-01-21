
namespace ParkingGarage.Common {
    public class LocationSettings {
        public string BaseRate { get; set; }
        public string RateDurations { get; set; }
        public string RateIncreasePercent { get; set; }
        public string Occupancy { get; set; }

        public LocationSettings() {
            //
        }
    }
}