using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class ExplicitContentFrame {
        private String timeOffset;
        private PornographicContentLikelihood pornographyLikelihood;

        [JsonProperty("timeOffset")]
        public string TimeOffset {
            get => timeOffset;
            set => timeOffset = value;
        }

        [JsonProperty("pornographyLikelihood")]
        public PornographicContentLikelihood PornographyLikelihood {
            get => pornographyLikelihood;
            set => pornographyLikelihood = value;
        }

        public ExplicitContentFrame(string timeOffset, PornographicContentLikelihood pornographyLikelihood) {
            this.TimeOffset = timeOffset;
            this.PornographyLikelihood = pornographyLikelihood;
        }
    }
}
