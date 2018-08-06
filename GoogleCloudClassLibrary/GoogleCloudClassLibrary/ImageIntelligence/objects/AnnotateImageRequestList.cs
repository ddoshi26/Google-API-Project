using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class AnnotateImageRequestList {
        private List<AnnotateImageRequest> requests;

        [JsonProperty("requests")]
        public List<AnnotateImageRequest> Requests {
            get => requests; set => requests = value;
        }

        public AnnotateImageRequestList(List<AnnotateImageRequest> requests) {
            this.Requests = requests;
        }
    }
}
