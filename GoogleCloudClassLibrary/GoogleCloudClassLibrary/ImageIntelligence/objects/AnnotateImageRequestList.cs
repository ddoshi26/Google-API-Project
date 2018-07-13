using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class AnnotateImageRequestList {
        private List<AnnotateImageRequests> requests;

        [JsonProperty("requests")]
        public List<AnnotateImageRequests> Requests {
            get => requests; set => requests = value;
        }

        public AnnotateImageRequestList(List<AnnotateImageRequests> requests) {
            this.Requests = requests;
        }
    }
}
