using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class DetectedLanguage {
        private String languageCode;
        private double confidence;

        [JsonProperty("languageCode")]
        public String LanguageCode {
            get => languageCode; set => languageCode = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public DetectedLanguage(String languageCode, double confidence) {
            this.LanguageCode = languageCode;
            this.Confidence = confidence;
        }
    }
}
