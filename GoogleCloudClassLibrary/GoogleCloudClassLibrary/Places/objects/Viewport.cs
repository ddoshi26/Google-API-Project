using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.Places {
    public class Viewport {
        private Location northEast;
        private Location southWest;

        [JsonProperty("northeast")]
        public Location NorthEast {
            get => northEast;
            set => northEast = value;
        }

        [JsonProperty("southwest")]
        public Location SouthWest {
            get => southWest;
            set => southWest = value;
        }

        public Viewport(Location northEast, Location southWest) {
            NorthEast = northEast;
            SouthWest = southWest;
        }
    }
}
