using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class AnalyzeSentimentResponse {
        private Sentiment sentiment;
        private String language;
        private List<Sentence> sentences;
        private Error error;

        [JsonProperty("documentSentiment")]
        public Sentiment Sentiment {
            get => sentiment; set => sentiment = value;
        }

        [JsonProperty("language")]
        public String Language {
            get => language; set => language = value;
        }

        [JsonProperty("sentences")]
        public List<Sentence> Sentences {
            get => sentences; set => sentences = value;
        }

        [JsonProperty("error")]
        public Error Error {
            get => error; set => error = value;
        }

        public AnalyzeSentimentResponse(Sentiment sentiment, String language, List<Sentence> sentences) {
            this.Sentiment = sentiment;
            this.Language = language;
            this.Sentences = sentences;
        }
    }
}
