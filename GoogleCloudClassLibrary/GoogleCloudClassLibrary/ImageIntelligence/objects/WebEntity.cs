using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class WebEntity {
        private String entityid;
        private String description;
        private double relevancyscore;

        [JsonProperty("entityId")]
        public String EntityId {
            get => entityid; set => entityid = value;
        }

        [JsonProperty("description")]
        public String Description {
            get => description; set => description = value;
        }

        [JsonProperty("score")]
        public double Relevancyscore {
            get => relevancyscore; set => relevancyscore = value;
        }

        public WebEntity(String entityId, String description, double score) {
            this.EntityId = entityId;
            this.Description = description;
            this.Relevancyscore = score;
        }
    }
}
