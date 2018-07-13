using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class Entity {
        private String entityId;
        private String description;
        private String languageCode;

        [JsonProperty("entityId")]
        public string EntityId {
            get => entityId;
            set => entityId = value;
        }

        [JsonProperty("description")]
        public string Description {
            get => description;
            set => description = value;
        }

        [JsonProperty("languageCode")]
        public string LanguageCode {
            get => languageCode;
            set => languageCode = value;
        }

        public Entity(string entityId, string description, string languageCode) {
            this.EntityId = entityId;
            this.Description = description;
            this.LanguageCode = languageCode;
        }
    }
}
