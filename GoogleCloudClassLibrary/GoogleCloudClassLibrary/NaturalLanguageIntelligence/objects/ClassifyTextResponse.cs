using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class ClassifyTextResponse {
        private List<ClassificationCategory> categories;
        private Error error;

        [JsonProperty("categories")]
        public List<ClassificationCategory> Categories {
            get => categories;
            set => categories = value;
        }

        [JsonProperty("error")]
        public Error Error {
            get => error;
            set => error = value;
        }

        public ClassifyTextResponse(List<ClassificationCategory> categories) {
            Categories = categories;
        }
    }
}