using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class AnnotationProgress {
        private String inputUri;
        private double progresspercent;
        private String startTime;
        private String updateTime;

        public string InputUri {
            get => inputUri;
            set => inputUri = value;
        }
        public double Progresspercent {
            get => progresspercent;
            set => progresspercent = value;
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
            Progresspercent = progresspercent;
            StartTime = startTime;
            UpdateTime = updateTime;
        }
    }
}
