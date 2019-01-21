
using ParkingGarage.Models;
using System.Threading.Tasks;

namespace ParkingGarage.Services {
    public interface ILocationService {

        int[] GetIntervalDurations();

        int GetOccupancy();

        double GetRate();
        
        double GetRateIncreasePercentage();

        bool TryEnterLocation();

        string ExitLocation(Ticket ticket);
    }
}