using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class Sentiment {
        private double magnitude;
        private double score;

        [JsonProperty("magnitude")]
        public double Magnitude {
            get => magnitude; set => magnitude = value;
        }

        [JsonProperty("score")]
        public double Score {
            get => score; set => score = value;
        }

        public Sentiment(double magnitude, double score) {
            this.Magnitude = magnitude;
            this.Score = score;
        }
    }
}
