using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class TextProperty {
        private List<DetectedLanguage> detectedLanguages;
        private DetectedBreak detectedBreak;

        [JsonProperty("detectedLanguages")]
        public List<DetectedLanguage> DetectedLanguages {
            get => detectedLanguages; set => detectedLanguages = value;
        }

        [JsonProperty("detectedBreak")]
        public DetectedBreak DetectedBreak {
            get => detectedBreak; set => detectedBreak = value;
        }

        public TextProperty(List<DetectedLanguage> detectedLanguages, DetectedBreak detectedBreak) {
            this.DetectedLanguages = detectedLanguages;
            this.DetectedBreak = detectedBreak;
        }
    }
}
