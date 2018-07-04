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
        private List<VideoSegement> segments;
        private LabelDetectionConfig labelDetection;
        private String shotChangeDetectionModel;
        private String explicitContentDetectionModel;

        public List<VideoSegement> Segements {
            get => segments; set => segments = value;
        }
        public LabelDetectionConfig LabelDetection {
            get => labelDetection; set => labelDetection = value;
        }
        public String ShotChangeDetectionModel {
            get => shotChangeDetectionModel; set => shotChangeDetectionModel = value;
        }
        public String ExplicitChangeDetectionModel {
            get => explicitContentDetectionModel; set => explicitContentDetectionModel = value;
        }

        public VideoContext(List<VideoSegement> segements, LabelDetectionConfig labelDetectionConfig, String shotChangeModel,
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
        private List<Object> details;

        public double Number {
            get => number; set => number = value;
        }
        public String Message {
            get => message; set => message = value;
        }
        public List<Object> Details {
            get => details; set => details = value;
        }

        public Status(double number, String message, List<Object> details) {
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
        public Status Status {
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
        private String input_Uri;
        private String input_Content;
        private List<VideoFeature> feats;
        private VideoContext video_Context;
        private String output_Uri;
        private String location_Id;

        public String inputUri {
            get => input_Uri; set => input_Uri = value;
        }
        public String inputContent {
            get => input_Content; set => input_Content = value;
        }
        public List<VideoFeature> features {
            get => feats; set => feats = value;
        }
        public VideoContext videoContext {
            get => video_Context; set => video_Context = value;
        }
        public String outputUri {
            get => output_Uri; set => output_Uri = value;
        }
        public String locationId {
            get => location_Id; set => location_Id = value;
        }

        public AnnotateVideoRequest(String inputUri, String inputContent, List<VideoFeature> videoFeatures,
            VideoContext videoContext, String outputUri, String outputLocationId) {
            if (!BasicFunctions.isEmpty(inputUri))
                this.inputUri = inputUri;
            if (!BasicFunctions.isEmpty(inputContent))
                this.inputContent = inputContent;
            this.features = videoFeatures;
            if (videoContext != null)
                this.videoContext = videoContext;
            if (!BasicFunctions.isEmpty(outputUri))
                this.outputUri = outputUri;
            if (!BasicFunctions.isEmpty(outputLocationId))
                this.locationId = outputLocationId;
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
        private String entity_Id;
        private String desc;
        private String language_Code;

        public string entityId {
            get => entity_Id;
            set => entity_Id = value;
        }
        public string description {
            get => desc;
            set => desc = value;
        }
        public string languageCode {
            get => language_Code;
            set => language_Code = value;
        }

        public Entity(string entityId, string description, string languageCode) {
            this.entityId = entityId;
            this.description = description;
            this.languageCode = languageCode;
        }
    }

    public class LabelSegment {
        private VideoSegement segment;
        private double conf;
        
        public VideoSegement Segment {
            get => segment;
            set => segment = value;
        }
        public double confidence {
            get => conf;
            set => conf = value;
        }

        public LabelSegment(VideoSegement segment, double confidence) {
            this.Segment = segment;
            this.confidence = confidence;
        }
    }

    public class LabelFrame {
        private String time_Offset;
        private double conf;

        public string timeOffset {
            get => time_Offset;
            set => time_Offset = value;
        }
        public double confidence {
            get => conf;
            set => conf = value;
        }

        public LabelFrame(String timeOffset, double confidence) {
            this.timeOffset = timeOffset;
            this.confidence = confidence;
        }
    }

    public class LabelAnnotation {
        private Entity entity;
        private List<Entity> category_Entities;
        private List<LabelSegment> segments;
        private List<LabelFrame> frame;

        public Entity Entity {
            get => entity;
            set => entity = value;
        }
        public List<Entity> categoryEntities {
            get => category_Entities;
            set => category_Entities = value;
        }
        public List<LabelSegment> Segments {
            get => segments;
            set => segments = value;
        }
        public List<LabelFrame> Frame {
            get => frame;
            set => frame = value;
        }

        public LabelAnnotation(Entity entity, List<Entity> categoryEntities, List<LabelSegment> segments, List<LabelFrame> frame) {
            this.entity = entity;
            this.categoryEntities = categoryEntities;
            this.segments = segments;
            this.frame = frame;
        }
    }

    public enum PornographicContentLikelihood {
        LIKELIHOOD_UNSPECIFIED, VERY_UNLIKELY, UNLIKELY, POSSIBLE, LIKELY, VERY_LIKELY
    }

    public class ExplicitContentFrame {
        private String time_Offset;
        private PornographicContentLikelihood pornography_Likelihood;
        public string timeOffset {
            get => time_Offset;
            set => time_Offset = value;
        }
        public PornographicContentLikelihood pornographyLikelihood {
            get => pornography_Likelihood;
            set => pornography_Likelihood = value;
        }

        public ExplicitContentFrame(string timeOffset, PornographicContentLikelihood pornographyLikelihood) {
            this.timeOffset = timeOffset;
            this.pornographyLikelihood = pornographyLikelihood;
        }
    }

    public class ExplicitContentAnnotation {
        private List<ExplicitContentFrame> frame;

        public List<ExplicitContentFrame> frames {
            get => frame;
            set => frame = value;
        }

        public ExplicitContentAnnotation(List<ExplicitContentFrame> frames) {
            this.frames = frames;
        }
    }

    public class VideoAnnotationResult {
        private String input_Uri;
        private List<LabelAnnotation> segmentLabelAnnotations;
        private List<LabelAnnotation> shotLabelAnnotations;
        private List<LabelAnnotation> frameLabelAnnotations;
        private List<VideoSegement> shotAnnotations;
        private ExplicitContentAnnotation explicitAnnotations;
        private Status error;

        public string inputUri {
            get => input_Uri;
            set => input_Uri = value;
        }
        public List<LabelAnnotation> SegmentLabelAnnotations {
            get => segmentLabelAnnotations;
            set => segmentLabelAnnotations = value;
        }
        public List<LabelAnnotation> ShotLabelAnnotations {
            get => shotLabelAnnotations;
            set => shotLabelAnnotations = value;
        }
        public List<LabelAnnotation> FrameLabelAnnotations {
            get => frameLabelAnnotations;
            set => frameLabelAnnotations = value;
        }
        public List<VideoSegement> ShotAnnotations {
            get => shotAnnotations;
            set => shotAnnotations = value;
        }
        public ExplicitContentAnnotation ExplicitAnnotation {
            get => explicitAnnotations;
            set => explicitAnnotations = value;
        }
        public Status Error {
            get => error;
            set => error = value;
        }

        public VideoAnnotationResult(string inputUri, List<LabelAnnotation> segmentLabelAnnotations, 
            List<LabelAnnotation> shotLabelAnnotations, List<LabelAnnotation> frameLabelAnnotations,
            List<VideoSegement> shotAnnotations, ExplicitContentAnnotation explicitAnnotations, Status error) {
            this.inputUri = inputUri;
            this.SegmentLabelAnnotations = segmentLabelAnnotations;
            this.ShotLabelAnnotations = shotLabelAnnotations;
            this.frameLabelAnnotations = frameLabelAnnotations;
            this.shotAnnotations = shotAnnotations;
            this.explicitAnnotations = explicitAnnotations;
            this.error = error;
        }
    }

    public class AnnotateResponse {
        private String type;
        private List<VideoAnnotationResult> annotation_Results;

        [JsonProperty("@type")]
        public string Type {
            get => type;
            set => type = value;
        }
        public List<VideoAnnotationResult> annotationResults {
            get => annotation_Results;
            set => annotation_Results = value;
        }

        public AnnotateResponse(string type, List<VideoAnnotationResult> annotationResults) {
            this.Type = type;
            this.annotationResults = annotationResults;
        }
    }

    public class VideoAnnotationResponse {
        private String name;
        private Metadata metadata;
        private Boolean done;
        private AnnotateResponse response;

        public string Name {
            get => name;
            set => name = value;
        }
        public Metadata Metadata {
            get => metadata;
            set => metadata = value;
        }
        public bool Done {
            get => done;
            set => done = value;
        }
        public AnnotateResponse Response {
            get => response;
            set => response = value;
        }

        public VideoAnnotationResponse(string name, Metadata metadata, bool done, AnnotateResponse response) {
            this.Name = name;
            this.Metadata = metadata;
            Done = done;
            this.Response = response;
        }
    }
}
