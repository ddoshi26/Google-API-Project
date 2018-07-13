using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class TextFeatures {
        private Boolean extractSyntax;
        private Boolean extractEntities;
        private Boolean extractDocumentSentiment;
        private Boolean extractEntitySentiment;
        private Boolean classifyText;

        [JsonProperty("extractSyntax")]
        public bool ExtractSyntax {
            get => extractSyntax; set => extractSyntax = value;
        }

        [JsonProperty("extractEntities")]
        public bool ExtractEntities {
            get => extractEntities; set => extractEntities = value;
        }

        [JsonProperty("extractDocumentSentiment")]
        public bool ExtractDocumentSentiment {
            get => extractDocumentSentiment; set => extractDocumentSentiment = value;
        }

        [JsonProperty("extractEntitySentiment")]
        public bool ExtractEntitySentiment {
            get => extractEntitySentiment; set => extractEntitySentiment = value;
        }

        [JsonProperty("classifyText")]
        public bool ClassifyText {
            get => classifyText; set => classifyText = value;
        }

        public TextFeatures(bool extractSyntax, bool extractEntities, bool extractDocumentSentiment,
            bool extractEntitySentiment, bool classifyText) {

            this.ExtractSyntax = extractSyntax;
            this.ExtractEntities = extractEntities;
            this.ExtractDocumentSentiment = extractDocumentSentiment;
            this.ExtractEntitySentiment = extractEntitySentiment;
            this.ClassifyText = classifyText;
        }
    }
}
