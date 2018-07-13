using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    // TODO: Check
    public class LocationInfo {
        private LatLng latLng;

        [JsonProperty("latLng")]
        public LatLng LatLng {
            get => latLng; set => latLng = value;
        }

        public LocationInfo(LatLng latLng) {
            this.LatLng = latLng;
        }
    }
}
