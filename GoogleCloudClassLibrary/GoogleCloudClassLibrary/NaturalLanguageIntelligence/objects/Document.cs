using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class Document {
        private DocumentType type;
        private String language;
        private String content;
        private String googleCloudUri;

        [JsonProperty("type")]
        public DocumentType Type {
            get => type; set => type = value;
        }
        [JsonProperty("language")]
        public String Language {
            get => language; set => language = value;
        }
        [JsonProperty("content")]
        public String Content {
            get => content; set => content = value;
        }
        [JsonProperty("gcsContentUri")]
        public String GoogleCloudUri {
            get => googleCloudUri; set => googleCloudUri = value;
        }

        public Document(DocumentType type, String language, String content = null, String googleCloudUri = null) {
            this.Type = type;
            this.Language = language;
            this.Content = content;
            this.GoogleCloudUri = googleCloudUri;
        }
    }
}
