using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class ExplicitContentDetectionConfig {
        private String model;

        [JsonProperty("model")]
        public String Model {
            get => model;
            set => model = value;
        }

        public ExplicitContentDetectionConfig(String model) {
            Model = model;
        }
    }
}
