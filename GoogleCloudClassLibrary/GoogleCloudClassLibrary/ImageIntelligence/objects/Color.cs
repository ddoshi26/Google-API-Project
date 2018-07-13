using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Color {
        private double red;
        private double green;
        private double blue;
        private double alpha;

        [JsonProperty("red")]
        public double Red {
            get => red; set => red = value;
        }

        [JsonProperty("green")]
        public double Green {
            get => green; set => green = value;
        }

        [JsonProperty("blue")]
        public double Blue {
            get => blue; set => blue = value;
        }

        [JsonProperty("alpha")]
        public double Alpha {
            get => alpha; set => alpha = value;
        }

        public Color(double red, double green, double blue, double alpha) {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }
    }
}
