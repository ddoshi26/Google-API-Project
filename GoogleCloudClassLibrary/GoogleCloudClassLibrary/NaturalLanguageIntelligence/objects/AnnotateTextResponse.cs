using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class AnnotateTextResponse {
        private List<Sentence> sentences;
        private List<Token> tokens;
        private List<Entity> entities;
        private Sentiment documentSentiment;
        private String language;
        private List<ClassificationCategory> categories;

        [JsonProperty("sentences")]
        public List<Sentence> Sentences {
            get => sentences; set => sentences = value;
        }

        [JsonProperty("tokens")]
        public List<Token> Tokens {
            get => tokens; set => tokens = value;
        }

        [JsonProperty("entities")]
        public List<Entity> Entities {
            get => entities; set => entities = value;
        }

        [JsonProperty("documentSentiment")]
        public Sentiment DocumentSentiment {
            get => documentSentiment; set => documentSentiment = value;
        }

        [JsonProperty("language")]
        public String Language {
            get => language; set => language = value;
        }

        [JsonProperty("categories")]
        public List<ClassificationCategory> Categories {
            get => categories; set => categories = value;
        }

        public AnnotateTextResponse(List<Sentence> sentences, List<Token> tokens, List<Entity> entities, Sentiment sentiment,
            String language, List<ClassificationCategory> classificationCategories) {
            this.Sentences = sentences;
            this.Tokens = tokens;
            this.Entities = entities;
            this.DocumentSentiment = sentiment;
            this.Language = language;
            this.Categories = classificationCategories;
        }
    }
}
