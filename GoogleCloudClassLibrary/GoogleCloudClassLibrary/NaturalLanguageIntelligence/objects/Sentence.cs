using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class Sentence {
        private TextSpan text;
        private Sentiment sentiment;

        [JsonProperty("text")]
        public TextSpan Text {
            get => text; set => text = value;
        }

        [JsonProperty("sentiment")]
        public Sentiment Sentiment {
            get => sentiment; set => sentiment = value;
        }

        public Sentence(TextSpan text, Sentiment sentiment) {
            this.Text = text;
            this.Sentiment = sentiment;
        }
    }
}
