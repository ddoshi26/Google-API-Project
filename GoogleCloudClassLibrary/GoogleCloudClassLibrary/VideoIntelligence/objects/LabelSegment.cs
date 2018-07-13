using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class LabelSegment {
        private VideoSegement segment;
        private double confidence;

        [JsonProperty("segment")]
        public VideoSegement Segment {
            get => segment;
            set => segment = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence;
            set => confidence = value;
        }

        public LabelSegment(VideoSegement segment, double confidence) {
            this.Segment = segment;
            this.Confidence = confidence;
        }
    }
}
