using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Paragraph {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private List<Word> words;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("boundingBox")]
        public BoundingPoly BoundingBox {
            get => boundingBox; set => boundingBox = value;
        }

        [JsonProperty("words")]
        public List<Word> Words {
            get => words; set => words = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Paragraph(TextProperty property, BoundingPoly boundingBox, List<Word> words, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Words = words;
            this.Confidence = confidence;
        }
    }
}
