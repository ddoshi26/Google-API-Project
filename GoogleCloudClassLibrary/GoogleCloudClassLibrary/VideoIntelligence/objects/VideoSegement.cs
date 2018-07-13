using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoSegement {
        private String startTimeOffset;
        private String endTimeOffset;

        [JsonProperty("startTimeOffset")]
        public String StartTimeOffset {
            get => startTimeOffset; set => startTimeOffset = value;
        }
        [JsonProperty("endTimeOffset")]
        public String EndTimeOffset {
            get => endTimeOffset; set => endTimeOffset = value;
        }

        public VideoSegement(String startOffset, String endOffset) {
            this.StartTimeOffset = startOffset;
            this.EndTimeOffset = endOffset;
        }
    }
}
