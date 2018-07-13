using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class Metadata {
        private String type;
        private List<AnnotationProgress> annotation_Progress;

        [JsonProperty("@type")]
        public String Type {
            get => type;
            set => type = value;
        }

        public List<AnnotationProgress> annotationProgress {
            get => annotation_Progress;
            set => annotation_Progress = value;
        }

        public Metadata(string type, List<AnnotationProgress> annotationProgress) {
            this.Type = type;
            this.annotationProgress = annotationProgress;
        }
    }
}
