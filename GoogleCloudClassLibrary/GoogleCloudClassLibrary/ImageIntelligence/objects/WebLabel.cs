using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class WebLabel {
        private String label;
        private String languageCode;

        [JsonProperty("label")]
        public String Label {
            get => label; set => label = value;
        }

        [JsonProperty("languageCode")]
        public String LanguageCode {
            get => languageCode; set => languageCode = value;
        }

        public WebLabel(String label, String languageCode) {
            this.Label = label;
            this.LanguageCode = languageCode;
        }
    }
}
