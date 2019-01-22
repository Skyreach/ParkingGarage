using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ParkingGarage.Models {
    public class Ticket : ModelBase {
        internal bool isPaid;
        
        [JsonProperty(PropertyName = "timeIssued")]
        public DateTime TimeIssued { get; set; }

        // Todo: extract into own class
        [JsonProperty(PropertyName = "licensePlate")]
        public string LicensePlate { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public double Rate { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; internal set; }

        [JsonProperty(PropertyName = "intervals")]
        public int[] Intervals { get; set; }

        [JsonProperty(PropertyName = "increasePercentage")]
        public double IncreasePercentage { get; set; }

        public double AmountOwing { get; set; }

        public Ticket() {
            TimeIssued = DateTime.Now;
            IsActive = true;
            isPaid = false;
        }

        public Ticket(Ticket ticket) {
            TimeIssued = ticket.TimeIssued;
            LicensePlate = ticket.LicensePlate;
            Rate = ticket.Rate;
            IsActive = ticket.IsActive;
            Intervals = ticket.Intervals;
            IncreasePercentage = ticket.IncreasePercentage;
            AmountOwing = ticket.AmountOwing;
            isPaid = ticket.isPaid;
        }
    }
}
