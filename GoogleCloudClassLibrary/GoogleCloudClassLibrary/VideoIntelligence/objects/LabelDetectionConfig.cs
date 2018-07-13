using System;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class LabelDetectionConfig {
        private Boolean shotMode = false;
        private Boolean frameMode = false;
        private Boolean stationaryCamera;
        private String model;

        public bool ShotMode {
            get => shotMode; set => shotMode = value;
        }
        public bool FrameMode {
            get => frameMode; set => frameMode = value;
        }

        public bool StationaryCamera {
            get => stationaryCamera; set => stationaryCamera = value;
        }
        public String Model {
            get => model; set => model = value;
        }

        public LabelDetectionConfig(bool shotMode, bool frameMode, bool stationaryCamera, String model) {
            this.ShotMode = shotMode;
            this.FrameMode = frameMode;
            this.StationaryCamera = stationaryCamera;
            this.Model = model;
        }
    }
}
