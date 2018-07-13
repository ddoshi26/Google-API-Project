using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class LatLng {
        private Double latitude;
        private Double longitude;

        [JsonProperty("latitude")]
        public double Latitude {
            get => latitude; set => latitude = value;
        }

        [JsonProperty("longitude")]
        public double Longitude {
            get => longitude; set => longitude = value;
        }

        public LatLng(Double lat, Double lng) {
            this.Latitude = lat;
            this.Longitude = lng;
        }
    }
}
