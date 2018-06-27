using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoSegement {
        private String startTimeOffset;
        private String endTimeOffset;

        public String StartTimeOffset {
            get => startTimeOffset; set => startTimeOffset = value;
        }
        public String EndTimeOffset {
            get => endTimeOffset; set => endTimeOffset = value;
        }

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

    public class VideoContext {
        private VideoSegement[] segments;
        private LabelDetectionConfig labelDetection;
        private String shotChangeDetectionModel;
        private String explicitContentDetectionModel;

        internal VideoSegement[] Segements {
            get => segments; set => segments = value;
        }
        internal LabelDetectionConfig LabelDetection {
            get => labelDetection; set => labelDetection = value;
        }
        public String ShotChangeDetectionModel {
            get => shotChangeDetectionModel; set => shotChangeDetectionModel = value;
        }
        public String ExplicitChangeDetectionModel {
            get => explicitContentDetectionModel; set => explicitContentDetectionModel = value;
        }

        public VideoContext(VideoSegement[] segements, LabelDetectionConfig labelDetectionConfig, String shotChangeModel,
            String explicitContentModel) {
            this.Segements = segements;
            this.LabelDetection = labelDetectionConfig;
            this.ShotChangeDetectionModel = shotChangeModel;
            this.ExplicitChangeDetectionModel = explicitContentModel;
        }
    }

    public class Status {
        private double number;
        private String message;
        private Object[] details;

        public double Number {
            get => number; set => number = value;
        }
        public String Message {
            get => message; set => message = value;
        }
        public Object[] Details {
            get => details; set => details = value;
        }

        public Status(double number, String message, Object[] details) {
            this.Number = number;
            this.Message = message;
            this.Details = details;
        }
    }

    public class Operation {
        private String name;
        private Object metadata;
        private Boolean operationDone;
        private Status status;
        private Object response;

        public String Name {
            get => name; set => name = value;
        }
        public Object Metadata {
            get => metadata; set => metadata = value;
        }
        public Boolean OperationDone {
            get => operationDone; set => operationDone = value;
        }
        internal Status Status {
            get => status; set => status = value;
        }
        public Object Response {
            get => response; set => response = value;
        }

        public Operation(String name, Object metadata, Boolean operationDone, Status status, Object response) {
            this.Name = name;
            this.Metadata = metadata;
            this.OperationDone = operationDone;
            this.Status = status;
            this.Response = response;
        }
    }

    public enum VideoFeature {
        FEATURE_UNSPECIFIED, LABEL_DETECTION, SHOT_CHANGE_DETECTION, EXPLICIT_CONTENT_DETECTION
    }

    public class AnnotateVideoRequest {
        private String inputUri;
        private String inputContent;
        private VideoFeature[] features;
        private VideoContext videoContext;
        private String outputUri;
        private String outputLocationId;

        public String InputUri {
            get => inputUri; set => inputUri = value;
        }
        public String InputContent {
            get => inputContent; set => inputContent = value;
        }
        internal VideoFeature[] Features {
            get => features; set => features = value;
        }
        internal VideoContext VideoContext {
            get => videoContext; set => videoContext = value;
        }
        public String OutputUri {
            get => outputUri; set => outputUri = value;
        }
        public String OutputLocationId {
            get => outputLocationId; set => outputLocationId = value;
        }

        public AnnotateVideoRequest(String inputUri, String inputContent, VideoFeature[] videoFeatures,
            VideoContext videoContext, String outputUri, String outputLocationId) {
            if (!BasicFunctions.isEmpty(inputUri))
                InputUri = inputUri;
            if (!BasicFunctions.isEmpty(inputContent))
                InputContent = inputContent;
            Features = videoFeatures;
            if (videoContext != null)
                VideoContext = videoContext;
            if (!BasicFunctions.isEmpty(outputUri))
                OutputUri = outputUri;
            if (!BasicFunctions.isEmpty(outputLocationId))
                OutputLocationId = outputLocationId;
        }

    }
}
