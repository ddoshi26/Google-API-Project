using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class EntityMention {
        private TextSpan text;
        private EntityMentionType type;
        private Sentiment sentiment;

        [JsonProperty("text")]
        public TextSpan Text {
            get => text; set => text = value;
        }

        [JsonProperty("type")]
        public EntityMentionType Type {
            get => type; set => type = value;
        }

        [JsonProperty("sentiment")]
        public Sentiment Sentiment {
            get => sentiment; set => sentiment = value;
        }

        public EntityMention(TextSpan text, EntityMentionType type, Sentiment sentiment) {
            this.Text = text;
            this.Type = type;
            this.Sentiment = sentiment;
        }
    }
}
