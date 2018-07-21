using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class DetectedBreak {
        private BreakType type;
        private Boolean isPrefix;

        [JsonProperty("type")]
        public BreakType Type {
            get => type; set => type = value;
        }

        [JsonProperty("isPrefix")]
        public Boolean IsPrefix {
            get => isPrefix; set => isPrefix = value;
        }

        public DetectedBreak(BreakType type, Boolean isPrefix) {
            this.Type = type;
            this.IsPrefix = isPrefix;
        }
    }
}
