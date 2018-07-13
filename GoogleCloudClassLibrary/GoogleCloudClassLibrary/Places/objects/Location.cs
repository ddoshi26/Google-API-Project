using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.Places {
    public class Location {
        private Double lat;
        private Double lng;

        [JsonProperty("lat")]
        public double Lat {
            get => lat; set => lat = value;
        }

        [JsonProperty("lng")]
        public double Lng {
            get => lng; set => lng = value;
        }

        public Location() {
            this.Lat = 0.00;
            this.Lng = 0.00;
        }

        public Location(Double lat, Double lng) {
            this.Lat = lat;
            this.Lng = lng;
        }
    }
}
