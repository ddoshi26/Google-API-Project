using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Symbol {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private String text;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("boundingBox")]
        public BoundingPoly BoundingBox {
            get => boundingBox; set => boundingBox = value;
        }

        [JsonProperty("text")]
        public String Text {
            get => text; set => text = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Symbol(TextProperty property, BoundingPoly boundingBox, String text, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Text = text;
            this.Confidence = confidence;
        }
    }
}
