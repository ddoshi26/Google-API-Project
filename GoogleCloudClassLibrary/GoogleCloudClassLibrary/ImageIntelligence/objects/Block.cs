using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Block {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private List<Paragraph> paragraphs;
        private BlockType blockType;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("boundingBox")]
        public BoundingPoly BoundingBox {
            get => boundingBox; set => boundingBox = value;
        }

        [JsonProperty("paragraphs")]
        public List<Paragraph> Paragraphs {
            get => paragraphs; set => paragraphs = value;
        }

        [JsonProperty("blockType")]
        public BlockType BlockType {
            get => blockType; set => blockType = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Block(TextProperty property, BoundingPoly boundingBox, List<Paragraph> paragraphs, BlockType blockType, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Paragraphs = paragraphs;
            this.BlockType = blockType;
            this.Confidence = confidence;
        }
    }
}
