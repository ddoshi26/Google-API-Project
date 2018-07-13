using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class ExplicitContentAnnotation {
        private List<ExplicitContentFrame> frames;

        [JsonProperty("frames")]
        public List<ExplicitContentFrame> Frames {
            get => frames;
            set => frames = value;
        }

        public ExplicitContentAnnotation(List<ExplicitContentFrame> frames) {
            this.Frames = frames;
        }
    }
}
