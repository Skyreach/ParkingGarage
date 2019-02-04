
namespace ParkingGarage.Common {
    public class LocationSettings {
        public double BaseRate { get; set; }
        public string RateDurations { get; set; }
        public double RateIncreasePercent { get; set; }
        public int Occupancy { get; set; }

        public LocationSettings() {
            //
        }
    }
}