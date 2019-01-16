using System.Threading.Tasks;

namespace ParkingGarage.Services {
    public interface ILocationService {

        int[] GetIntervalDurations();

        double GetRate();
        
        double GetRateIncreasePercentage();
    }
}