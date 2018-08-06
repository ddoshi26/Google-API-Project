using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class AnnotateVideoRequest {
        private String inputUri;
        private String inputContent;
        private List<String> features;
        private VideoContext videoContext;
        private String outputUri;
        private String locationId;

        [JsonProperty("inputUri")]
        public String InputUri {
            get => inputUri; set => inputUri = value;
        }

        [JsonProperty("inputContent")]
        public String InputContent {
            get => inputContent; set => inputContent = value;
        }

        [JsonProperty("features")]
        public List<String> Features {
            get => features; set => features = value;
        }

        [JsonProperty("videoContext")]
        public VideoContext VideoContext {
            get => videoContext; set => videoContext = value;
        }

        [JsonProperty("outputUri")]
        public String OutputUri {
            get => outputUri; set => outputUri = value;
        }

        [JsonProperty("locationId")]
        public String LocationId {
            get => locationId; set => locationId = value;
        }

        public AnnotateVideoRequest(String inputUri, String inputContent, List<String> videoFeatures,
            VideoContext videoContext, String outputUri, String outputLocationId) {
            if (!BasicFunctions.isEmpty(inputUri))
                this.InputUri = inputUri;
            if (!BasicFunctions.isEmpty(inputContent))
                this.InputContent = inputContent;
            this.Features = videoFeatures;
            if (videoContext != null)
                this.VideoContext = videoContext;
            if (!BasicFunctions.isEmpty(outputUri))
                this.OutputUri = outputUri;
            if (!BasicFunctions.isEmpty(outputLocationId))
                this.LocationId = outputLocationId;
        }
    }
}
