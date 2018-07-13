using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Vertex {
        private double x;
        private double y;

        [JsonProperty("x")]
        public double X {
            get => x; set => x = value;
        }

        [JsonProperty("y")]
        public double Y {
            get => y; set => y = value;
        }

        public Vertex(double x, double y) {
            this.X = x;
            this.Y = y;
        }
    }
}
