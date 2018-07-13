using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoContext {
        private List<VideoSegement> segments;
        private LabelDetectionConfig labelDetectionConfig;
        private ShotChangeDetectionConfig shotChangeDetectionConfig;
        private ExplicitContentDetectionConfig explicitContentDetectionConfig;

        [JsonProperty("segments")]
        public List<VideoSegement> Segements {
            get => segments; set => segments = value;
        }

        [JsonProperty("labelDetectionConfig")]
        public LabelDetectionConfig LabelDetectionConfig {
            get => labelDetectionConfig; set => labelDetectionConfig = value;
        }

        [JsonProperty("shotChangeDetectionConfig")]
        public ShotChangeDetectionConfig ShotChangeDetectionConfig {
            get => shotChangeDetectionConfig; set => shotChangeDetectionConfig = value;
        }
        public ExplicitContentDetectionConfig ExplicitContentDetectionConfig {
            get => explicitContentDetectionConfig; set => explicitContentDetectionConfig = value;
        }

        public VideoContext(List<VideoSegement> segements, LabelDetectionConfig labelDetectionConfig, 
            ShotChangeDetectionConfig shotChangeDetectionConfig, ExplicitContentDetectionConfig explicitContentDetectionConfig) {
            this.Segements = segements;
            this.LabelDetectionConfig = labelDetectionConfig;
            this.ShotChangeDetectionConfig = shotChangeDetectionConfig;
            this.ExplicitContentDetectionConfig = explicitContentDetectionConfig;
        }
    }
}
