using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.VideoIntelligence {
    public class VideoSegement {
        private String startTimeOffset;
        private String endTimeOffset;

        [JsonProperty("startTimeOffset")]
        public String StartTimeOffset {
            get => startTimeOffset; set => startTimeOffset = value;
        }
        [JsonProperty("endTimeOffset")]
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

    public class ShotChangeDetectionConfig {
        private String model;

        [JsonProperty("model")]
        public String Model {
            get => model;
            set => model = value;
        }

        public ShotChangeDetectionConfig(String model) {
            Model = model;
        }
    }


    public class ExplicitContentDetectionConfig {
        private String model;

        [JsonProperty("model")]
        public String Model {
            get => model;
            set => model = value;
        }

        public ExplicitContentDetectionConfig(String model) {
            Model = model;
        }
    }

    public class VideoContext {
        private List<VideoSegement> segments;
        private LabelDetectionConfig labelDetectionConfig;
        private ShotChangeDetectionConfig shotChangeDetectionConfig;
        private ExplicitContentDetectionConfig explicitContentDetectionConfig;

        [JsonProperty("segments")]
        public List<VideoSegement> Segements {
            get => segments; set => segments = value;
        }

        [JsonProperty("labelDetectionConfig")]
        public LabelDetectionConfig LabelDetectionConfig {
            get => labelDetectionConfig; set => labelDetectionConfig = value;
        }

        [JsonProperty("shotChangeDetectionConfig")]
        public ShotChangeDetectionConfig ShotChangeDetectionConfig {
            get => shotChangeDetectionConfig; set => shotChangeDetectionConfig = value;
        }
        public ExplicitContentDetectionConfig ExplicitContentDetectionConfig {
            get => explicitContentDetectionConfig; set => explicitContentDetectionConfig = value;
        }

        public VideoContext(List<VideoSegement> segements, LabelDetectionConfig labelDetectionConfig, 
            ShotChangeDetectionConfig shotChangeDetectionConfig, ExplicitContentDetectionConfig explicitContentDetectionConfig) {
            this.Segements = segements;
            this.LabelDetectionConfig = labelDetectionConfig;
            this.ShotChangeDetectionConfig = shotChangeDetectionConfig;
            this.ExplicitContentDetectionConfig = explicitContentDetectionConfig;
        }
    }

    public class Status {
        private double code;
        private String message;
        private List<Object> details;

        [JsonProperty("code")]
        public double Code {
            get => code; set => code = value;
        }

        [JsonProperty("message")]
        public String Message {
            get => message; set => message = value;
        }

        [JsonProperty("details")]
        public List<Object> Details {
            get => details; set => details = value;
        }

        public Status(double code, String message, List<Object> details) {
            this.Code = code;
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

        [JsonProperty("name")]
        public String Name {
            get => name; set => name = value;
        }

        [JsonProperty("metadata")]
        public Object Metadata {
            get => metadata; set => metadata = value;
        }

        [JsonProperty("done")]
        public Boolean OperationDone {
            get => operationDone; set => operationDone = value;
        }

        [JsonProperty("status")]
        public Status Status {
            get => status; set => status = value;
        }

        [JsonProperty("response")]
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
        private List<VideoFeature> features;
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
        public List<VideoFeature> Features {
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

        public AnnotateVideoRequest(String inputUri, String inputContent, List<VideoFeature> videoFeatures,
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

    public class Entity {
        private String entityId;
        private String description;
        private String languageCode;

        [JsonProperty("entityId")]
        public string EntityId {
            get => entityId;
            set => entityId = value;
        }

        [JsonProperty("description")]
        public string Description {
            get => description;
            set => description = value;
        }

        [JsonProperty("languageCode")]
        public string LanguageCode {
            get => languageCode;
            set => languageCode = value;
        }

        public Entity(string entityId, string description, string languageCode) {
            this.EntityId = entityId;
            this.Description = description;
            this.LanguageCode = languageCode;
        }
    }

    public class LabelSegment {
        private VideoSegement segment;
        private double confidence;

        [JsonProperty("segment")]
        public VideoSegement Segment {
            get => segment;
            set => segment = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence;
            set => confidence = value;
        }

        public LabelSegment(VideoSegement segment, double confidence) {
            this.Segment = segment;
            this.Confidence = confidence;
        }
    }

    public class LabelFrame {
        private String timeOffset;
        private double confidence;

        [JsonProperty("timeOffset")]
        public string TimeOffset {
            get => timeOffset;
            set => timeOffset = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence;
            set => confidence = value;
        }

        public LabelFrame(String timeOffset, double confidence) {
            this.TimeOffset = timeOffset;
            this.Confidence = confidence;
        }
    }

    public class LabelAnnotation {
        private Entity entity;
        private List<Entity> categoryEntities;
        private List<LabelSegment> segments;
        private List<LabelFrame> frames;

        [JsonProperty("entity")]
        public Entity Entity {
            get => entity;
            set => entity = value;
        }

        [JsonProperty("categoryEntities")]
        public List<Entity> CategoryEntities {
            get => categoryEntities;
            set => categoryEntities = value;
        }

        [JsonProperty("segments")]
        public List<LabelSegment> Segments {
            get => segments;
            set => segments = value;
        }

        [JsonProperty("frames")]
        public List<LabelFrame> Frames {
            get => frames;
            set => frames = value;
        }

        public LabelAnnotation(Entity entity, List<Entity> categoryEntities, List<LabelSegment> segments, List<LabelFrame> frame) {
            this.Entity = entity;
            this.CategoryEntities = categoryEntities;
            this.Segments = segments;
            this.Frames = frame;
        }
    }

    public enum PornographicContentLikelihood {
        LIKELIHOOD_UNSPECIFIED, VERY_UNLIKELY, UNLIKELY, POSSIBLE, LIKELY, VERY_LIKELY
    }

    public class ExplicitContentFrame {
        private String timeOffset;
        private PornographicContentLikelihood pornographyLikelihood;

        [JsonProperty("timeOffset")]
        public string TimeOffset {
            get => timeOffset;
            set => timeOffset = value;
        }

        [JsonProperty("pornographyLikelihood")]
        public PornographicContentLikelihood PornographyLikelihood {
            get => pornographyLikelihood;
            set => pornographyLikelihood = value;
        }

        public ExplicitContentFrame(string timeOffset, PornographicContentLikelihood pornographyLikelihood) {
            this.TimeOffset = timeOffset;
            this.PornographyLikelihood = pornographyLikelihood;
        }
    }

    public class ExplicitContentAnnotation {
        private List<ExplicitContentFrame> frames;

        [JsonProperty("frames")]
        public List<ExplicitContentFrame> Frames {
            get => frames;
            set => frames = value;
        }

        public ExplicitContentAnnotation(List<ExplicitContentFrame> frames) {
            this.Frames = frames;
        }
    }

    public class VideoAnnotationResult {
        private String inputUri;
        private List<LabelAnnotation> segmentLabelAnnotations;
        private List<LabelAnnotation> shotLabelAnnotations;
        private List<LabelAnnotation> frameLabelAnnotations;
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

        [JsonProperty("frameLabelAnnotations")]
        public List<LabelAnnotation> FrameLabelAnnotations {
            get => frameLabelAnnotations;
            set => frameLabelAnnotations = value;
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
            List<LabelAnnotation> shotLabelAnnotations, List<LabelAnnotation> frameLabelAnnotations,
            List<VideoSegement> shotAnnotations, ExplicitContentAnnotation explicitAnnotation, Status error) {
            this.InputUri = inputUri;
            this.SegmentLabelAnnotations = segmentLabelAnnotations;
            this.ShotLabelAnnotations = shotLabelAnnotations;
            this.FrameLabelAnnotations = frameLabelAnnotations;
            this.ShotAnnotations = shotAnnotations;
            this.ExplicitAnnotation = explicitAnnotation;
            this.Error = error;
        }
    }

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

    public class VideoAnnotationResponse {
        private String name;
        private Metadata metadata;
        private Boolean done;
        private AnnotateResponse response;

        [JsonProperty("name")]
        public string Name {
            get => name;
            set => name = value;
        }

        [JsonProperty("metadata")]
        public Metadata Metadata {
            get => metadata;
            set => metadata = value;
        }

        [JsonProperty("done")]
        public bool Done {
            get => done;
            set => done = value;
        }

        [JsonProperty("response")]
        public AnnotateResponse Response {
            get => response;
            set => response = value;
        }

        public VideoAnnotationResponse(string name, Metadata metadata, bool done, AnnotateResponse response) {
            this.Name = name;
            this.Metadata = metadata;
            this.Done = done;
            this.Response = response;
        }
    }
}
