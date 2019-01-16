using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarage.Models {
    public class Vehicle : ModelBase {
        public string LicensePlate { get; set; }
        public string VehicleName { get; set; }
    }
}
