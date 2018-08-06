using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class AnnotationProgress {
        private String inputUri;
        private double progressPercent;
        private String startTime;
        private String updateTime;

        public string InputUri {
            get => inputUri;
            set => inputUri = value;
        }
        public double ProgressPercent {
            get => progressPercent;
            set => progressPercent = value;
        }
        public string StartTime {
            get => startTime;
            set => startTime = value;
        }
        public string UpdateTime {
            get => updateTime;
            set => updateTime = value;
        }

        public AnnotationProgress(String inputUri, double progresspercent, String startTime, String updateTime) {
            InputUri = inputUri;
            ProgressPercent = progresspercent;
            StartTime = startTime;
            UpdateTime = updateTime;
        }
    }
}
