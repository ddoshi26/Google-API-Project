using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.Places {
    public class Geometry {
        private Location location;
        private Viewport viewport;

        [JsonProperty("location")]
        public Location Location {
            get => location;
            set => location = value;
        }

        [JsonProperty("viewport")]
        public Viewport Viewport {
            get => viewport;
            set => viewport = value;
        }

        public Geometry(Location location, Viewport viewport) {
            Location = location;
            Viewport = viewport;
        }
    }
}
