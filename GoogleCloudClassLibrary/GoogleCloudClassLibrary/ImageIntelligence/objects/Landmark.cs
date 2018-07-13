using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Landmark {
        private LandmarkType type;
        private Position position;

        [JsonProperty("type")]
        public LandmarkType Type {
            get => type; set => type = value;
        }

        [JsonProperty("position")]
        public Position Position {
            get => position; set => position = value;
        }

        public Landmark(LandmarkType type, Position position) {
            this.Type = type;
            this.Position = position;
        }
    }
}
