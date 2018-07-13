using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class CropHint {
        private BoundingPoly boundingPoly;
        private double confidence;
        private double importanceFraction;

        [JsonProperty("boundingPoly")]
        public BoundingPoly BoundingPoly {
            get => boundingPoly; set => boundingPoly = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        [JsonProperty("importanceFraction")]
        public double ImportanceFraction {
            get => importanceFraction; set => importanceFraction = value;
        }

        public CropHint(BoundingPoly boundingPoly, double confidence, double importanceFraction) {
            this.BoundingPoly = boundingPoly;
            this.Confidence = confidence;
            this.ImportanceFraction = importanceFraction;
        }
    }
}
