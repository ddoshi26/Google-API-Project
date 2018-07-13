using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class AnalyzeEntitiesResponse {
        private List<Entity> entities;
        private String language;

        [JsonProperty("entities")]
        public List<Entity> Entities {
            get => entities; set => entities = value;
        }

        [JsonProperty("language")]
        public String Language {
            get => language; set => language = value;
        }

        public AnalyzeEntitiesResponse(List<Entity> entities, String language) {
            this.Entities = entities;
            this.Language = language;
        }
    }
}
