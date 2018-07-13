using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class CropHintsParams {
        private List<double> aspectRatios;

        [JsonProperty("aspectRatios")]
        public List<double> AspectRatios {
            get => aspectRatios;
            set => aspectRatios = value;
        }

        public CropHintsParams(List<double> aspectRatios) {
            AspectRatios = aspectRatios;
        }
    }
}
