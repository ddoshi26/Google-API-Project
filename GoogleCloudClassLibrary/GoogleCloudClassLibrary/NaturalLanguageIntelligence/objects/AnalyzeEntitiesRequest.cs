using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class AnalyzeEntitiesRequest {
        private Document document;
        private EncodingType encodingType;

        [JsonProperty("document")]
        public Document Document { get => document; set => document = value; }

        [JsonProperty("encodingType")]
        public EncodingType EncodingType { get => encodingType; set => encodingType = value; }

        public AnalyzeEntitiesRequest(Document document, EncodingType encodingType) {
            Document = document;
            EncodingType = encodingType;
        }
    }
}
