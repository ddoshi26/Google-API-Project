using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Page {
        private TextProperty property;
        private double width;
        private double height;
        private List<Block> blocks;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("width")]
        public double Width {
            get => width; set => width = value;
        }

        [JsonProperty("height")]
        public double Height {
            get => height; set => height = value;
        }

        [JsonProperty("blocks")]
        public List<Block> Blocks {
            get => blocks; set => blocks = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Page(TextProperty property, double width, double height, List<Block> blocks, double confidence) {
            this.Property = property;
            this.Width = width;
            this.Height = height;
            this.Blocks = blocks;
            this.Confidence = confidence;
        }
    }
}
