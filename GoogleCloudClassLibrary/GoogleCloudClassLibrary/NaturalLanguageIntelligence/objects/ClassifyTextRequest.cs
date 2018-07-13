using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class ClassifyTextRequest {
        private Document document;

        [JsonProperty("document")]
        public Document Document {
            get => document;
            set => document = value;
        }

        public ClassifyTextRequest(Document document) {
            Document = document;
        }
    }
}
