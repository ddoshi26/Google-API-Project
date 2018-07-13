using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class AnnotateResponse {
        private String type;
        private List<VideoAnnotationResult> annotationResults;

        [JsonProperty("@type")]
        public string Type {
            get => type;
            set => type = value;
        }

        [JsonProperty("annotationResults")]
        public List<VideoAnnotationResult> AnnotationResults {
            get => annotationResults;
            set => annotationResults = value;
        }

        public AnnotateResponse(string type, List<VideoAnnotationResult> annotationResults) {
            this.Type = type;
            this.AnnotationResults = annotationResults;
        }
    }
}
