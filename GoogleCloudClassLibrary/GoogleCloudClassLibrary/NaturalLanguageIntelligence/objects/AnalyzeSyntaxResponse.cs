using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class AnalyzeSyntaxResponse {
        private List<Sentence> sentences;
        private List<Token> tokens;
        private String language;

        [JsonProperty("sentences")]
        public List<Sentence> Sentences {
            get => sentences; set => sentences = value;
        }

        [JsonProperty("tokens")]
        public List<Token> Tokens {
            get => tokens; set => tokens = value;
        }

        [JsonProperty("language")]
        public String Language {
            get => language; set => language = value;
        }

        public AnalyzeSyntaxResponse(List<Sentence> sentences, List<Token> tokens, String language) {
            this.Sentences = sentences;
            this.Tokens = tokens;
            this.Language = language;
        }
    }
}
