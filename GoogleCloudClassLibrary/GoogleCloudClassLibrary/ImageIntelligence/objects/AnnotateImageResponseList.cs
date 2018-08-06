using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class AnnotateImageResponseList {
        private List<AnnotateImageResponse> responses;
        private Error error;

        [JsonProperty("responses")]
        public List<AnnotateImageResponse> Responses {
            get => responses;
            set => responses = value;
        }

        [JsonProperty("error")]
        public Error Error {
            get => error;
            set => error = value;
        }

        public AnnotateImageResponseList(List<AnnotateImageResponse> responses) {
            Responses = responses;
        }
    }
}
