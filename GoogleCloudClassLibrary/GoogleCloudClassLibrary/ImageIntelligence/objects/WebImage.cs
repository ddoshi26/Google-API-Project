using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class WebImage {
        private String url;
        private double score;

        [JsonProperty("url")]
        public String URL {
            get => url; set => url = value;
        }

        [JsonProperty("score")]
        public double Score {
            get => score; set => score = value;
        }

        public WebImage(String url, double score) {
            this.URL = url;
            this.Score = score;
        }
    }
}
