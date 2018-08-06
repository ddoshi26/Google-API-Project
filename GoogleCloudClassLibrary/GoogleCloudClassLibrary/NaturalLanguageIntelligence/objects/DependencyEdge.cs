using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class DependencyEdge {
        private int headTokenIndex;
        private DependencyEdgeLabel label;

        [JsonProperty("headTokenIndex")]
        public int Token {
            get => headTokenIndex;
            set => headTokenIndex = value;
        }

        [JsonProperty("label")]
        public DependencyEdgeLabel Label {
            get => label;
            set => label = value;
        }

        public DependencyEdge(int token, DependencyEdgeLabel label) {
            Token = token;
            Label = label;
        }
    }
}
