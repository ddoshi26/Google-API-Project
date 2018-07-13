using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Word {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private List<Symbol> symbols;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("boundingBox")]
        public BoundingPoly BoundingBox {
            get => boundingBox; set => boundingBox = value;
        }

        [JsonProperty("symbols")]
        public List<Symbol> Symbols {
            get => symbols; set => symbols = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Word(TextProperty property, BoundingPoly boundingBox, List<Symbol> symbols, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Symbols = symbols;
            this.Confidence = confidence;
        }
    }
}
