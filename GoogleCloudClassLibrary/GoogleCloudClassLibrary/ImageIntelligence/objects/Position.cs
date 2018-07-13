using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Position {
        double x;
        double y;
        double z;

        [JsonProperty("x")]
        public double X {
            get => x; set => x = value;
        }

        [JsonProperty("y")]
        public double Y {
            get => y; set => y = value;
        }

        [JsonProperty("z")]
        public double Z {
            get => z; set => z = value;
        }

        public Position(double x, double y, double z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
