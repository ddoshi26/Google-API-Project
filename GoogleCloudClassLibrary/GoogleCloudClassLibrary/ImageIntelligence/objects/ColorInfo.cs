using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class ColorInfo {
        private Color color;
        private double score;
        private double pixelFraction;

        [JsonProperty("color")]
        public Color Color {
            get => color; set => color = value;
        }

        [JsonProperty("score")]
        public double Score {
            get => score; set => score = value;
        }

        [JsonProperty("pixelFraction")]
        public double PixelFraction {
            get => pixelFraction; set => pixelFraction = value;
        }

        public ColorInfo(Color color, double score, double pixelFraction) {
            this.Color = color;
            this.Score = score;
            this.PixelFraction = pixelFraction;
        }
    }
}
