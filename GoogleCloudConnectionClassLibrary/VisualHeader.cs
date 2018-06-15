using System;
using GoogleCloudClassLibrary;

namespace GoogleCloudClassLibrary.VisualHeader {
    public class VideoSegement {
        private String startTimeOffset;
        private String endTimeOffset;

        public String StartTimeOffset { get => startTimeOffset; set => startTimeOffset = value; }
        public String EndTimeOffset { get => endTimeOffset; set => endTimeOffset = value; }

        public VideoSegement(String startOffset, String endOffset) {
            this.StartTimeOffset = startOffset;
            this.EndTimeOffset = endOffset;
        }
    }

    public class LabelDetectionConfig {
        private Boolean shotMode = false;
        private Boolean frameMode = false;

        private Boolean stationaryCamera;
        private String model;

        public bool ShotMode { get => shotMode; set => shotMode = value; }
        public bool FrameMode { get => frameMode; set => frameMode = value; }
        
        public bool StationaryCamera { get => stationaryCamera; set => stationaryCamera = value; }
        public String Model { get => model; set => model = value; }

        public LabelDetectionConfig(bool shotMode, bool frameMode, bool stationaryCamera, String model) {
            this.ShotMode = shotMode;
            this.FrameMode = frameMode;
            this.StationaryCamera = stationaryCamera;
            this.Model = model;
        }
    }

    public class VideoContext {
        private VideoSegement[] segments;
        private LabelDetectionConfig labelDetection;
        private String shotChangeDetectionModel;
        private String explicitContentDetectionModel;

        internal VideoSegement[] Segements { get => segments; set => segments = value; }
        internal LabelDetectionConfig LabelDetection { get => labelDetection; set => labelDetection = value; }        
        public String ShotChangeDetectionModel { get => shotChangeDetectionModel; set => shotChangeDetectionModel = value; }
        public String ExplicitChangeDetectionModel { get => explicitContentDetectionModel; set => explicitContentDetectionModel = value; }

        public VideoContext(VideoSegement[] segements, LabelDetectionConfig labelDetectionConfig, String shotChangeModel, 
            String explicitContentModel) {
            this.Segements = segements;
            this.LabelDetection = labelDetectionConfig;
            this.ShotChangeDetectionModel = shotChangeModel;
            this.ExplicitChangeDetectionModel = explicitContentModel;
        }
    }

    public class Operation {
        String name;
        Object metadata;
        Boolean operationDone;
        Object response;
    }
}