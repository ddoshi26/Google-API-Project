using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class LabelFrame {
        private String timeOffset;
        private double confidence;

        [JsonProperty("timeOffset")]
        public string TimeOffset {
            get => timeOffset;
            set => timeOffset = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence;
            set => confidence = value;
        }

        public LabelFrame(String timeOffset, double confidence) {
            this.TimeOffset = timeOffset;
            this.Confidence = confidence;
        }
    }
}
