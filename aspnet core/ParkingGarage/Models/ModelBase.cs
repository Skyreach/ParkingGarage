﻿using System;
using Newtonsoft.Json;

namespace ParkingGarage.Models {

    public class ModelBase {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt { get; set; }

        public ModelBase() {
            CreatedAt = DateTime.UtcNow;
            Id = Guid.NewGuid().ToString();
        }
    }
}
