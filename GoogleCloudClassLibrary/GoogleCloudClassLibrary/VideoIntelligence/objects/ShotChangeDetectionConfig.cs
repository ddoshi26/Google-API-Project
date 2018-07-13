using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class ShotChangeDetectionConfig {
        private String model;

        [JsonProperty("model")]
        public String Model {
            get => model;
            set => model = value;
        }

        public ShotChangeDetectionConfig(String model) {
            Model = model;
        }
    }
}
