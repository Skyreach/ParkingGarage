using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ParkingGarage.Models {
    public class Ticket : ModelBase {

        [JsonProperty(PropertyName = "timeIssued")]
        public DateTime TimeIssued { get; internal set; }

        // Todo: extract into own class
        [JsonProperty(PropertyName = "licensePlate")]
        public string LicensePlate { get; internal set; }

        [JsonProperty(PropertyName = "rate")]
        public double Rate { get; internal set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "intervals")]
        public int[] Intervals { get; internal set; }

        [JsonProperty(PropertyName = "increasePercentage")]
        public double IncreasePercentage { get; internal set; }

        public double AmountOwing { get; set; }

        public Ticket() {
            TimeIssued = DateTime.Now;
            IsActive = true;
        }
    }
}
