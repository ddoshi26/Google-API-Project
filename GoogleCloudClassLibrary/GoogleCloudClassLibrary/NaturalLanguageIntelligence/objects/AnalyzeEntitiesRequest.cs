using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class AnalyzeEntitiesRequest {
        private Document document;
        private String encodingType;

        [JsonProperty("document")]
        public Document Document { get => document; set => document = value; }

        [JsonProperty("encodingType")]
        public String EncodingType { get => encodingType; set => encodingType = value; }

        public AnalyzeEntitiesRequest(Document document, EncodingType encodingType) {
            Document = document;
            EncodingType = encodingType.ToString();
        }
    }
}
