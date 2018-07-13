using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class AnnotateTextRequest {
        private Document document;
        private TextFeatures features;
        private EncodingType encodingType;

        [JsonProperty("document")]
        public Document Document {
            get => document;
            set => document = value;
        }

        [JsonProperty("features")]
        public TextFeatures Features {
            get => features;
            set => features = value;
        }

        [JsonProperty("encodingType")]
        public EncodingType EncodingType {
            get => encodingType;
            set => encodingType = value;
        }

        public AnnotateTextRequest(Document document, TextFeatures features, EncodingType encodingType) {
            Document = document;
            Features = features;
            EncodingType = encodingType;
        }
    }
}
