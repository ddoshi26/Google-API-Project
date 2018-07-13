using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class LatLongRect {
        private LatLng minLatLng;
        private LatLng maxLatLng;

        [JsonProperty("minLatLng")]
        public LatLng MinLatLng {
            get => minLatLng; set => minLatLng = value;
        }

        [JsonProperty("maxLatLng")]
        public LatLng MaxLatLng {
            get => maxLatLng; set => maxLatLng = value;
        }

        public LatLongRect(LatLng minLatLng, LatLng maxLatLng) {
            this.MinLatLng = minLatLng;
            this.MaxLatLng = maxLatLng;
        }
    }
}
