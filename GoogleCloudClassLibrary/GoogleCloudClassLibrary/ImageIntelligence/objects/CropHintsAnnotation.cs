using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class CropHintsAnnotation {
        private List<CropHint> cropHints;

        [JsonProperty("cropHints")]
        public List<CropHint> CropHints {
            get => cropHints; set => cropHints = value;
        }

        public CropHintsAnnotation(List<CropHint> cropHints) {
            this.CropHints = cropHints;
        }
    }
}
