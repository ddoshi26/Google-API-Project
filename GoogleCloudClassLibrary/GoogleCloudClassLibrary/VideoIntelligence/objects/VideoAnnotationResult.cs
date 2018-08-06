using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoAnnotationResult {
        private String inputUri;
        private List<LabelAnnotation> segmentLabelAnnotations;
        private List<LabelAnnotation> shotLabelAnnotations;
        private List<VideoSegement> shotAnnotations;
        private ExplicitContentAnnotation explicitAnnotation;
        private Status error;

        [JsonProperty("inputUri")]
        public string InputUri {
            get => inputUri;
            set => inputUri = value;
        }

        [JsonProperty("segmentLabelAnnotations")]
        public List<LabelAnnotation> SegmentLabelAnnotations {
            get => segmentLabelAnnotations;
            set => segmentLabelAnnotations = value;
        }

        [JsonProperty("shotLabelAnnotations")]
        public List<LabelAnnotation> ShotLabelAnnotations {
            get => shotLabelAnnotations;
            set => shotLabelAnnotations = value;
        }

        [JsonProperty("shotAnnotations")]
        public List<VideoSegement> ShotAnnotations {
            get => shotAnnotations;
            set => shotAnnotations = value;
        }

        [JsonProperty("explicitAnnotation")]
        public ExplicitContentAnnotation ExplicitAnnotation {
            get => explicitAnnotation;
            set => explicitAnnotation = value;
        }

        [JsonProperty("error")]
        public Status Error {
            get => error;
            set => error = value;
        }

        public VideoAnnotationResult(string inputUri, List<LabelAnnotation> segmentLabelAnnotations, 
            List<LabelAnnotation> shotLabelAnnotations, List<VideoSegement> shotAnnotations,
            ExplicitContentAnnotation explicitAnnotation, Status error) {
            this.InputUri = inputUri;
            this.SegmentLabelAnnotations = segmentLabelAnnotations;
            this.ShotLabelAnnotations = shotLabelAnnotations;
            this.ShotAnnotations = shotAnnotations;
            this.ExplicitAnnotation = explicitAnnotation;
            this.Error = error;
        }
    }
}
