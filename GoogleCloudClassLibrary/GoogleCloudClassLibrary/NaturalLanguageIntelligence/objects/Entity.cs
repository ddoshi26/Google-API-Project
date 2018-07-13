using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class Entity {
        private String name;
        private EntityResponseType type;
        private Dictionary<String, String> metadata;
        private double salience;
        private List<EntityMention> mentions;
        private Sentiment sentiment;

        [JsonProperty("name")]
        public String Name {
            get => name; set => name = value;
        }

        [JsonProperty("type")]
        public EntityResponseType Type {
            get => type; set => type = value;
        }

        [JsonProperty("metadata")]
        public Dictionary<String, String> Metadata {
            get => metadata; set => metadata = value;
        }

        [JsonProperty("salience")]
        public double Salience {
            get => salience; set => salience = value;
        }

        [JsonProperty("mentions")]
        public List<EntityMention> Mentions {
            get => mentions; set => mentions = value;
        }

        [JsonProperty("sentiment")]
        public Sentiment Sentiment {
            get => sentiment; set => sentiment = value;
        }

        public Entity(String name, EntityResponseType type, Dictionary<String, String> metadata, double salience,
            List<EntityMention> mentions, Sentiment sentiment) {
            this.Name = name;
            this.Type = type;
            this.Metadata = metadata;
            this.Salience = salience;
            this.Mentions = mentions;
            this.Sentiment = sentiment;
        }
    }
}
