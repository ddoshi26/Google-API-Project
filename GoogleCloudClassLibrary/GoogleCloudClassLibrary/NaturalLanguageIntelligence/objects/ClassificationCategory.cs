using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class ClassificationCategory {
        private String name;
        private double confidence;

        [JsonProperty("name")]
        public String Name {
            get => name; set => name = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public ClassificationCategory(String name, double confidence) {
            this.Name = name;
            this.Confidence = confidence;
        }
    }
}
