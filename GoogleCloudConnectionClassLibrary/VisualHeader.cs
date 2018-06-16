using System;
using GC = GoogleCloudClassLibrary.PlacesHeader;

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
    
    public class Status {
        private double number;
        private String message;
        private Object[] details;

        public double Number { get => number; set => number = value; }
        public String Message { get => message; set => message = value; }
        public Object[] Details { get => details; set => details = value; }

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

        public String Name { get => name; set => name = value; }
        public Object Metadata { get => metadata; set => metadata = value; }
        public Boolean OperationDone { get => operationDone; set => operationDone = value; }
        internal Status Status { get => status; set => status = value; }
        public Object Response { get => response; set => response = value; }

        public Operation(String name, Object metadata, Boolean operationDone, Status status, Object response) {
            this.Name = name;
            this.Metadata = metadata;
            this.OperationDone = operationDone;
            this.Status = status;
            this.Response = response;
        }
    }

    public enum DocumentType {
        HTML, PLAIN_TEXT
    }

    public enum EncodingType {
        NONE, UTF8, UTF16, UTF32
    }

    public class Document {
        private DocumentType type;
        private String language;
        private String content;
        private String googleCloudUri = "";

        internal DocumentType Type { get => type; set => type = value; }
        public String Language { get => language; set => language = value; }
        public String Content { get => content; set => content = value; }
        public String GoogleCloudUri { get => googleCloudUri; set => googleCloudUri = value; }

        public Document(DocumentType type, String language, String content) {
            this.Type = type;
            this.Language = language;
            this.Content = content;
        }
    }

    public class TextFeatures {
        private Boolean extractSyntax;
        private Boolean extractEntities;
        private Boolean extractDocumentSentiment;
        private Boolean extractEntitySentiment;
        private Boolean classifyText;

        public bool ExtractSyntax { get => extractSyntax; set => extractSyntax = value; }
        public bool ExtractEntities { get => extractEntities; set => extractEntities = value; }
        public bool ExtractDocumentSentiment { get => extractDocumentSentiment; set => extractDocumentSentiment = value; }
        public bool ExtractEntitySentiment { get => extractEntitySentiment; set => extractEntitySentiment = value; }
        public bool ClassifyText { get => classifyText; set => classifyText = value; }

        public TextFeatures(bool extractSyntax, bool extractEntities, bool extractDocumentSentiment, 
            bool extractEntitySentiment, bool classifyText) {
            
            this.ExtractSyntax = extractSyntax;
            this.ExtractEntities = extractEntities;
            this.ExtractDocumentSentiment = extractDocumentSentiment;
            this.ExtractEntitySentiment = extractEntitySentiment;
            this.ClassifyText = classifyText;
        }
    }

    public class Sentiment {
        private double magnitude;
        private double score;

        public double Magnitude { get => magnitude; set => magnitude = value; }
        public double Score { get => score; set => score = value; }

        public Sentiment(double magnitude, double score) {
            this.Magnitude = magnitude;
            this.Score = score;
        }
    }

    public class TextSpan {
        String content;
        int beginOffset;

        public String Content { get => content; set => content = value; }
        public int BeginOffset { get => beginOffset; set => beginOffset = value; }

        public TextSpan(String content, int beginOffset) {
            this.Content = content;
            this.BeginOffset = beginOffset;
        }
    }

    public enum EntityResponseType {
        UNKNOWN, PERSON, LOCATION, ORGANIZATION, EVENT, WORK_OF_ART, CONSUMER_GOOD, OTHER
    }

    public enum EntityMentionType {
        TYPE_UNKNOWN, PROPER, COMMON
    }

    public class EntityMention {
        private TextSpan text;
        private EntityMentionType type;
        private Sentiment sentiment;

        internal TextSpan Text { get => text; set => text = value; }
        internal EntityMentionType Type { get => type; set => type = value; }
        internal Sentiment Sentiment { get => sentiment; set => sentiment = value; }

        public EntityMention(TextSpan text, EntityMentionType type, Sentiment sentiment) {
            this.Text = text;
            this.Type = type;
            this.Sentiment = sentiment;
        }
    }

    public class Entity {
        private String name;
        private EntityResponseType type;
        private Dictionary<String, String> metadata;
        private double salience;
        private EntityMention[] mentions;
        private Sentiment sentiment;

        public String Name { get => name; set => name = value; }
        internal EntityResponseType Type { get => type; set => type = value; }
        public Dictionary<String, String> Metadata { get => metadata; set => metadata = value; }
        public double Salience { get => salience; set => salience = value; }
        internal EntityMention[] Mentions { get => mentions; set => mentions = value; }
        internal Sentiment Sentiment { get => sentiment; set => sentiment = value; }

        public Entity(String name, EntityResponseType type, Dictionary<String, String> metadata, double salience, 
            EntityMention[] mentions, Sentiment sentiment) {
            this.Name = name;
            this.Type = type;
            this.Metadata = metadata;
            this.Salience = salience;
            this.Mentions = mentions;
            this.Sentiment = sentiment;
        }
    }

    public class AnalyzeEntitiesResponse {
        private Entity[] entities;
        private String language;

        internal Entity[] Entities { get => entities; set => entities = value; }
        public String Language { get => language; set => language = value; }

        public AnalyzeEntitiesResponse(Entity[] entities, String language) {
            this.Entities = entities;
            this.language = language;
        }
    }

    public class Sentence {
        private TextSpan text;
        private Sentiment sentiment;

        public TextSpan Text { get => text; set => text = value; }
        public Sentiment Sentiment { get => sentiment; set => sentiment = value; }

        public Sentence(TextSpan text, Sentiment sentiment) {
            this.Text = text;
            this.Sentiment = sentiment;
        }
    }

    public class AnalyzeSentimentResponse {
        private Sentiment sentiment;
        private String language;
        private Sentence[] sentences;

        public Sentiment Sentiment { get => sentiment; set => sentiment = value; }
        public String Language { get => language; set => language = value; }
        public Sentence[] Sentences { get => sentences; set => sentences = value; }

        public AnalyzeSentimentResponse(Sentiment sentiment, String language, Sentence[] sentences) {
            this.Sentiment = sentiment;
            this.Language = language;
            this.Sentences = sentences;
        }
    }

    public enum DependencyEdgeLabel {
        UNKNOWN, ABBREV, ACOMP, ADVCL, ADVMOD, AMOD, APPOS, ATTR, AUX, AUXPASS, CC, CCOMP, CONJ, CSUBJ, CSUBJPASS,
        DEP, DET, DISCOURSE, DOBJ, EXPL, GOESWITH, IOBJ, MARK, MWE, MWV, NEG, NN, NPADVMOD, NSUBJ, NSUBPJPASS, 
        NUM, NUMBER, P, PARATAXIS, PARTMOD, PCOMP, POBJ, POSS, POSTNEG, PRECOMP, PRECONJ, PREDET, PREF, PREP,
        PRONL, PRT, PS, QUANTMOD, RCMOD, RCMODREL, RDROP, REF,REMNANT, REPARANDUM, ROOT, SNUM, SUFF, TMOD, TOPIC, 
        VMOD, VOCATIVE, XCOMP, SUFFIX, TITLE, ADVPHMOD, AUXCAUS, AUXVV, DTMOD, FOREIGN, KW, LIST, NOMC, NOMCSUBJ, 
        NOMCSUBJPASS, NUMC, COP, DISLOCATES, ASP, GMOD, GOBJ, INFMOD, MES, NCOMP
    }

    public enum PartsOfSpeechTag {
        UNKNOWN, ADJ, ADP, ADV, CONJ, DET, NOUN, NUM, PRON, PRT, PUNCT, VERB, X, AFFIX
    }

    public enum PartsOfSpeechAspect {
        ASPECT_UNKNOWN, PERFECTIVE, IMPERFECTIVE, PROGRESSIVE
    }

    public enum PartsOfSpeechCase {
        CASE_UNKNOWN, ACCUSATIVE, ADVERBIAL, COMPLEMENTIVE, DATIVE, GENITIVE, INSTRUMENTAL, LOCATIVE, NOMINATIVE, 
        OBLIQUE, PARTITIVE, PREPOSITIONAL, REFLEXIVE_CASE, RELATIVE_CASE, VOCATIVE
    }

    public enum PartsOfSpeechForm {
        FORM_UNKNOWN, ADNOMIAL, AUXILIARY, COMPLEMENTIZER, FINAL_ENDING, GERUND, REALIS, IRREALIS, SHORT, LONG, ORDER, SPECIFIC
    }

    public enum PartsOfSpeechGender {
        GENDER_UNKOWN, FEMININE, MASCULINE, NEUTER
    }

    public enum PartsOfSpeechMood {
        MOOD_UNKNOWN, CONDITIONAL_MOOD, IMPERATIVE, INDICATIVE, INTERROGATIVE, JUSSIVE, SUBJUNCTIVE
    }

    public enum PartsOfSpeechNumber {
        NUMBER_UNKNOWN, SINGULAR, PLURAL, DUAL
    }

    public enum PartsOfSpeechPerson {
        PERSON_UNKNOWN, FIRST, SECOND, THIRD, REFLEXIVE_PERSON
    }

    public enum PartsOfSpeechProper {
        PROPER_UNKNOWN, PROPER, NOT_PROPER 
    }

    public enum PartsOfSpeechReciprocity {
        RECIPROCITY_UNKNOWN, RECIPROCAL, NON_RECIPROCAL
    }

    public enum PartsOfSpeechTense {
        TENSE_UNKNOWN, CONDITIONAL_TENSE, FUTURE, PAST, PRESENT, IMPERFECT, PLUPERFECT
    }

    public enum PartsOfSpeechVoice {
        VOICE_UNKNOWN, ACTIVE, CAUSATIVE, PASSIVE
    }

    public class PartsOfSpeech {
        PartsOfSpeechTag tag;
        PartsOfSpeechAspect aspect;
        PartsOfSpeechCase p_case;
        PartsOfSpeechForm form;
        PartsOfSpeechGender gender;
        PartsOfSpeechMood mood;
        PartsOfSpeechNumber number;
        PartsOfSpeechPerson person;
        PartsOfSpeechProper proper;
        PartsOfSpeechReciprocity reciprocity;
        PartsOfSpeechTense tense;
        PartsOfSpeechVoice voice;

        internal PartsOfSpeechTag Tag { get => tag; set => tag = value; }
        internal PartsOfSpeechAspect Aspect { get => aspect; set => aspect = value; }
        internal PartsOfSpeechTag Case { get => p_case; set => p_case = value; }
        internal PartsOfSpeechTag Form { get => form; set => form = value; }
        internal PartsOfSpeechTag Gender { get => gender; set => gender = value; }
        internal PartsOfSpeechTag Mood { get => mood; set => mood = value; }
        internal PartsOfSpeechTag Number { get => number; set => number = value; }
        internal PartsOfSpeechTag Person { get => person; set => person = value; }
        internal PartsOfSpeechTag Proper { get => proper; set => proper = value; }
        internal PartsOfSpeechTag Reciprocity { get => reciprocity; set => reciprocity = value; }
        internal PartsOfSpeechTag Tense { get => tense; set => tense = value; }
        internal PartsOfSpeechTag Voice { get => voice; set => voice = value; }
        
        public PartsOfSpeech(PartsOfSpeechTag tag, PartsOfSpeechAspect aspect, PartsOfSpeechCase p_case, 
            PartsOfSpeechForm form, PartsOfSpeechGender gender, PartsOfSpeechMood mood, PartsOfSpeechNumber number, 
            PartsOfSpeechPerson person, PartsOfSpeechProper proper, PartsOfSpeechReciprocity reciprocity, 
            PartsOfSpeechTense tense, PartsOfSpeechVoice voice) {
            this.Tag = tag;
            this.Aspect = aspect;
            this.Case = p_case;
            this.Form = form;
            this.Gender = gender;
            this.Mood = mood;
            this.Number = number;
            this.Person = person;
            this.Proper = proper;
            this.Reciprocity = reciprocity;
            this.Tense = tense;
            this.Voice = voice;
        }
    }

    public class Token {
        private TextSpan text;
        private PartsOfSpeech partsOfSpeech;
        private DependencyEdgeLabel dependencyEdge;
        private String lemma;

        internal TextSpan Text { get => text; set => text = value; }
        internal PartsOfSpeech PartsOfSpeech { get => partsOfSpeech; set => partsOfSpeech = value; }
        internal DependencyEdgeLabel DependencyEdge { get => dependencyEdge; set => dependencyEdge = value; }
        public String Lemma { get => lemma; set => lemma = value; }

        public Token(TextSpan text, PartsOfSpeech partsOfSpeech, DependencyEdgeLabel dependencyEdge, String lemma) {
            this.Text = text;
            this.PartsOfSpeech = partsOfSpeech;
            this.DependencyEdge = dependencyEdge;
            this.Lemma = lemma;
        }
    }

    public class AnalyzeSyntaxResponse {
        private Sentence[] sentences;
        private Token[] tokens;
        private String language;

        internal Sentence[] Sentences { get => sentences; set => sentences = value; }
        internal Token[] Tokens { get => tokens; set => tokens = value; }
        public String Language { get => language; set => language = value; }

        public AnalyzeSyntaxResponse(Sentence[] sentences, Token[] tokens, String language) {
            this.Sentences = sentences;
            this.Tokens = tokens;
            this.Language = language;
        }
    }

    public class ClassificationCategory {
        private String name;
        private double confidence;

        public String Name { get => name; set => name = value; }
        public double Confidence { get => confidence; set => confidence = value; }

        public ClassificationCategory(String name, double confidence) {
            this.Name = name;
            this.Confidence = confidence;
        }
    }
    
    public class AnnotateText {
        private Sentence[] sentences;
        private Token[] tokens;
        private Entity[] entities;
        private Sentiment documentSentiment;
        private String language;
        private ClassificationCategory[] categories;

        internal Sentence[] Sentences { get => sentences; set => sentences = value; }
        internal Token[] Tokens { get => tokens; set => tokens = value; }
        internal Entity[] Entities { get => entities; set => entities = value; }
        internal Sentiment DocumentSentiment { get => documentSentiment; set => documentSentiment = value; }
        public String Language { get => language; set => language = value; }
        internal ClassificationCategory[] Categories { get => categories; set => categories = value; }

        public AnnotateText(Sentence[] sentences, Token[] tokens, Entity[] entities, Sentiment sentiment, 
            String language, ClassificationCategory[] classificationCategories) {
            this.Sentences = sentences;
            this.Tokens = tokens;
            this.Entities = entities;
            this.DocumentSentiment = sentiment;
            this.Language = language;
            this.Categories = classificationCategories;

        }
    }

    // ImageIntelligence

    public class Image {
        private String content;
        private String sourceUri;

        public String Content { get => content; set => content = value; }
        public String SourceUri { get => sourceUri; set => sourceUri = value; }

        public Image(String content = "", String sourceUri = "") {
            this.Content = content;
            this.SourceUri = sourceUri;
        }
    }

    public enum ImageType {
        FACE_DETECTION, LANDMARK_DETECTION, LOGO_DETECTION, LABEL_DETECTION, TEXT_DETECTION, DOCUMENT_TEXT_DETECTION, 
        SAFE_SEARCH_DETECTION, IMAGE_PROPERTIES, CROP_HINTS, WEB_DETECTION
    }

    public class ImageFeatures {
        private ImageType type;
        private int maxResults;
        private String model;

        internal ImageType Type { get => type; set => type = value; }
        public int MaxResults { get => maxResults; set => maxResults = value; }
        public String Model { get => model; set => model = value; }

        public ImageFeatures(ImageType type, int maxResults, String model) {
            this.Type = type;
            this.maxResults = maxResults;
            this.model = model;
        }
    }

    public class LatLngRect {
        GC.Location minLatLng;
        GC.Location MaxLatLng;

        internal GC.Location MinLatLng { get => minLatLng; set => minLatLng = value; }
        internal GC.Location MaxLatLng { get => maxLatLng; set => maxLatLng = value; }

        public LatLngRect(GC.Location MinLatLng, GC.Location MaxLatLng) {
            this.MinLatLng = MinLatLng;
            this.MaxLatLng = MaxLatLng;
        }
    }

    public class ImageContext {
        private LatLongRect latLngRect;
        private String[] languageHints;
        private double[] cropHintAspectRatios;
        private Boolean includeGeoResults;

        internal LatLongRect LatLngRect { get => latLngRect; set => latLngRect = value; }
        public String[] LanguageHints { get => languageHints; set => languageHints = value; }
        public double[] CropHintAspectRatios { get => cropHintAspectRatios; set => cropHintAspectRatios = value; }
        public Boolean IncludeGeoResults { get => includeGeoResults; set => includeGeoResults = value; }

        public ImageContext(LatLngRect latLngRect, String[] langHints, double[] cropHintAspectRatios, Boolean includeGeoResults) {
            this.LatLngRect = latLngRect;
            this.LanguageHints = langHints;
            this.CropHintAspectRatios = cropHintAspectRatios;
            this.IncludeGeoResults = includeGeoResults;
        }
    }

    public class AnnotateImageRequests {
        private Image image;
        private ImageFeatures[] features;
        private ImageContext imageContext;

        public Image Image { get => image; set => image = value; }
        public ImageFeatures Features { get => features; set => features = value; }
        public ImageContext ImageContext { get => imageContext; set => imageContext = value; }

        public AnnotateImageRequests(Image image, ImageFeatures[] features, ImageContext context) {
            this.Image = image;
            this.Features = features;
            this.ImageContext = context;
        }
    }

    public class Vertex {
        private double x;
        private double y;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public Vertex(double x, double y) {
            this.X = x;
            this.Y = y;
        }
    }

    public class BoundingPoly {
        private Vertex[] vertices;

        internal Vertex[] Vertices { get => vertices; set => vertices = value; }

        public BoundingPoly(Vertex[] vertices) {
            this.Vertices = vertices;
        }
    }

    public enum LandmarkType {
        UNKNOWN_LANDMARK, LEFT_EYE, RIGHT_EYE, LEFT_OF_LEFT_EYEBROW, RIGHT_OF_LEFT_EYEBROW, LEFT_OF_RIGHT_EYEBROW, RIGHT_OF_RIGHT_EYEBROW, MIDPOINT_BETWEEN_EYES, NOSE_TIP, UPPER_LIP, LOWER_LIP, MOUTH_LEFT, MOUTH_RIGHT, MOUTH_CENTER, NOSE_BOTTOM_RIGHT, NOSE_BOTTOM_LEFT, NOSE_BOTTOM_CENTER, LEFT_EYE_TOP_BOUNDARY, LEFT_EYE_RIGHT_CORNER, LEFT_EYE_LEFT_CORNER, RIGHT_EYE_TOP_BOUNDARY, RIGHT_EYE_RIGHT_CORNER, RIGHT_EYE_BOTTOM_BOUNDARY, RIGHT_EYE_LEFT_CORNER, LEFT_EYEBROW_UPPER_MIDPOINT, RIGHT_EYEBROW_UPPER_MIDPOINT, LEFT_EAR_TRAGION, RIGHT_EAR_TRAGION, LEFT_EYE_PUPIL, RIGHT_EYE_PUPIL, FOREHEAD_GLABELLA, CHIN_GNATHION, CHIN_LEFT_GONION, CHIN_RIGHT_GONION
    }

    public class Position {
        double x;
        double y;
        double z;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }

        public Vertex(double x, double y, double z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    public enum Likelihood {
        UNKNOWN, VERY_UNLIKELY, UNLIKELY, POSSIBLE, LIKELY, VERY_LIKELY
    }

    public class Landmark {
        private LandmarkType type;
        private Position position;

        internal LandmarkType Type { get => type; set => type = value; }
        internal Position Position { get => position; set => position = value; }

        public Landmark(LandmarkType type, Position position) {
            this.Type = type;
            this.Position = position;
        }
    }

    public class FaceAnnotation {
        private BoundingPoly boundingPoly;
        private BoundingPoly fdBoundingPoly;
        private Landmark[] landmarks;
        private double rollAngle;
        private double panAngle;
        private double tiltAngle;
        private double detectionConfidence;
        private double landmarkingConfidence;
        private Likelihood joyLikelihood;
        private Likelihood sorrowLikelihood;
        private Likelihood angerLikelihood;
        private Likelihood surpriseLikelihood;
        private Likelihood underExposedLikelihood;
        private Likelihood blurredLikelihood;
        private Likelihood headwearLikelihood;

        internal BoundingPoly BoundingPoly { get => boundingPoly; set => boundingPoly = value; }
        internal BoundingPoly FDBoundingPoly { get => fdBoundingPoly; set => fdBoundingPoly = value; }
        internal Landmark[] Landmarks { get => landmarks; set => landmarks = value; }
        public double RollAngle { get => rollAngle; set => rollAngle = value; }
        public double PanAngle { get => panAngle; set => panAngle = value; }
        public double TiltAngle { get => tiltAngle; set => tiltAngle = value; }
        public double DetectionConfidence { get => detectionConfidence; set => detectionConfidence = value; }
        public double LandmarkingConfidence { get => landmarkingConfidence; set => landmarkingConfidence = value; }
        internal Likelihood JoyLikelihood { get => joyLikelihood; set => joyLikelihood = value; }
        internal Likelihood SorrowLikelihood { get => sorrowLikelihood; set => sorrowLikelihood = value; }
        internal Likelihood AngerLikelihood { get => angerLikelihood; set => angerLikelihood = value; }
        internal Likelihood SurpriseLikelihood { get => surpriseLikelihood; set => surpriseLikelihood = value; }
        internal Likelihood UnderExposedLikelihood { get => underExposedLikelihood; set => underExposedLikelihood = value; }
        internal Likelihood BlurredLikelihood { get => blurredLikelihood; set => blurredLikelihood = value; }
        internal Likelihood HeadwearLikelihood { get => headwearLikelihood; set => headwearLikelihood = value; }

        public FaceAnnotation(BoundingPoly boundingPoly, BoundingPoly fdBoundingPoly, Landmark[] landmarks, double rollAngle, double panAngle, double tiltAngle, double detectionConfidence, double landmarkingConfidence, Likelihood joyLikelihood, Likelihood sorrowLikelihood, Likelihood angerLikelihood, Likelihood surpriseLikelihood, Likelihood underExposedLikelihood, Likelihood blurredLikelihood, Likelihood headwearLikelihood) {
            this.BoundingPoly = boundingPoly;
            this.FDBoundingPoly = fdBoundingPoly;
            this.Landmarks = landmarks;
            this.RollAngle = rollAngle;
            this.PanAngle = panAngle;
            this.DetectionConfidence = detectionConfidence;
            this.LandmarkingConfidence = landmarkingConfidence;
            this.JoyLikelihood = joyLikelihood;
            this.SorrowLikelihood = sorrowLikelihood;
            this.AngerLikelihood = angerLikelihood;
            this.SurpriseLikelihood = surpriseLikelihood;
            this.UnderExposedLikelihood = underExposedLikelihood;
            this.BlurredLikelihood = blurredLikelihood;
            this.HeadwearLikelihood = headwearLikelihood;
        }
    }

    public class LocationInfo {
        private GC.Location latLng;

        public GC.Location LatLng { get => latLng; set => latLng = value; }

        public LocationInfo(GC.Location latLng) {
            this.LatLng = latLng;
        }
    }

    public class Property {
        private String name;
        private String val;
        private String uint64Value;

        public String Name { get => name; set => name = value; }
        public String Value { get => val; set => val = value; }
        public String Uint64Value { get => uint64Value; set => uint64Value = value; }

        public Property(String name, String value, String uint64Value) {
            this.Name = name;
            this.Value = value;
            this.Uint64Value = uint64Value;
        }
    }

    public class EntityAnnotation {
        private String mid;
        private String locale;
        private String description;
        private double score;
        private double confidence;
        private double topicality;
        private BoundingPoly boundingPoly;
        private LocationInfo[] locations;
        private Property[] properties;

        public String Mid { get => mid; set => mid = value; }
        public String Locale { get => locale; set => locale = value; }
        public String Description { get => description; set => description = value; }
        public double Score { get => score; set => score = value; }
        public double Confidence { get => confidence; set => confidence = value; }
        public double Topicality { get => topicality; set => topicality = value; }
        internal BoundingPoly BoundingPoly { get => boundingPoly; set => boundingPoly = value; }
        internal LocationInfo[] Locations { get => locations; set => locations = value; }
        internal Property[] Properties { get => properties; set => properties = value; }

        public EntityAnnotation(String mid, String locale, String description, double score, double confidence, 
            double topicality, BoundingPoly boundingPoly, LocationInfo[] locations, Property[] properties) {
            this.Mid = mid;
            this.Locale = locale;
            this.Description = description;
            this.Score = score;
            this.Confidence = confidence;
            this.Topicality = topicality;
            this.BoundingPoly = boundingPoly;
            this.Locations = locations;
            this.Properties = properties;
        }
    }

    public class DetectedLanguage {
        private String languageCode;
        private double confidence;

        public String LanguageCode { get => languageCode; set => languageCode = value; }
        public double Confidence { get => confidence; set => confidence = value; }

        public DetectedLanguage(String languageCode, double confidence) {
            this.LanguageCode = languageCode;
            this.Confidence = confidence;
        }
    }

    public enum BreakType {
        UNKNOWN, SPACE, SURE_SPACE, EOL_SURE_SPACE, HYPHEN, LINE_BREAK
    }

    public class DetectedBreak {
        private BreakType type;
        private Boolean isPrefix;

        internal BreakType Type { get => type; set => type = value; }
        public Boolean IsPrefix { get => isPrefix; set => isPrefix = value; }

        public DetectedBreak(BreakType type, Boolean isPrefix) {
            this.Type = type;
            this.IsPrefix = isPrefix;
        }
    }

    public class TextProperty {
        private DetectedLanguage[] detectedLanguages;
        private DetectedBreak detectedBreak;

        internal DetectedLanguage[] DetectedLanguages { get => detectedLanguages; set => detectedLanguages = value; }
        internal DetectedBreak DetectedBreak { get => detectedBreak; set => detectedBreak = value; }

        public TextProperty(DetectedLanguage[] detectedLanguages, DetectedBreak detectedBreak) {
            this.DetectedLanguages = detectedLanguages;
            this.DetectedBreak = detectedBreak;
        }
    }

    public class Symbol {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private String text;
        private double confidence;

        internal TextProperty Property { get => property; set => property = value; }
        internal BoundingPoly BoundingBox { get => boundingBox; set => boundingBox = value; }
        public String Text { get => text; set => text = value; }
        public double Confidence { get => confidence; set => confidence = value; }

        public Symbol(TextProperty property, BoundingPoly boundingBox, String text, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Text = text;
            this.Confidence = confidence;
        }
    }

    public class Word {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private Symbol[] symbols;
        private double confidence;

        internal TextProperty Property { get => property; set => property = value; }
        internal BoundingPoly BoundingBox { get => boundingBox; set => boundingBox = value; }
        internal Symbol[] Symbols { get => symbols; set => symbols = value; }
        public double Confidence { get => confidence; set => confidence = value; }

        public Symbol(TextProperty property, BoundingPoly boundingBox, Symbol[] symbols, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Symbols = symbols;
            this.Confidence = confidence;
        }
    }

    public class Paragraph {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private Word[] words;
        private double confidence;

        internal TextProperty Property { get => property; set => property = value; }
        internal BoundingPoly BoundingBox { get => boundingBox; set => boundingBox = value; }
        internal Word[] Words { get => words; set => words = value; }
        public double Confidence { get => confidence; set => confidence = value; }

        public Paragraph(TextProperty property, BoundingPoly boundingBox, Word[] words, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Words = words;
            this.Confidence = confidence;
        }
    }

    public enum BlockType {
        UNKNOWN, TEXT, TABLE, PICTURE, RULER, BARCODE
    }

    public class Block {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private Paragraph[] paragraphs;
        private BlockType blockType;
        private double confidence;

        internal TextProperty Property { get => property; set => property = value; }
        internal BoundingPoly BoundingBox { get => boundingBox; set => boundingBox = value; }
        internal Paragraph[] Paragraphs { get => paragraphs; set => paragraphs = value; }
        internal BlockType BlockType { get => blockType; set => blockType = value; }
        public double Confidence { get => confidence; set => confidence = value; }

        public Block(TextProperty property, BoundingPoly boundingBox, Paragraph[] paragraphs, BlockType blockType, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Paragraphs = paragraphs;
            this.BlockType = blockType;
            this.Confidence = confidence;
        }
    }

    public class Page {
        private TextProperty property;
        private double width;
        private double height;
        private Block[] blocks;
        private double confidence;

        internal TextProperty Property { get => property; set => property = value; }
        public double Width { get => width; set => width = value; }
        public double Height { get => height; set => height = value;}
        internal Block[] Blocks { get => blocks; set => blocks = value; }
        public double Confidence { get => confidence; set => confidence = value; }

        public Page(TextProperty property, double width, double height, Block[] blocks, double confidence) {
            this.Property = property;
            this.Width = width;
            this.Height = height;
            this.Blocks = blocks;
            this.Confidence = confidence;
        }
    }

    public class TextAnnotation {
        private Page[] pages;
        private String text;

        public Page[] Pages { get => pages; set => pages = value; }
        public String Text { get => text; set => text = value; }

        public TextAnnotation(Page[] pages, String text) {
            this.Pages = pages;
            this.Text = text;
        }
    }

    public class SafeSearchAnnotation {
        private Likelihood adult;
        private Likelihood spoof;
        private Likelihood medical;
        private Likelihood violence;
        private Likelihood racy;

        public Likelihood Adult { get => adult; set => adult = value; }
        public Likelihood Spoof { get => spoof; set => spoof = value; }
        public Likelihood Medical { get => medical; set => medical = value; }
        public Likelihood Violence { get => violence; set => violence = value; }
        public Likelihood Racy { get => racy; set => racy = value; }

        public SafeSearchAnnotation(Likelihood adult, Likelihood spoof, Likelihood medical, Likelihood violence, Likelihood racy) {
            this.Adult = adult;
            this.Spoof = spoof;
            this.Medical = medical;
            this.Violence = violence;
            this.Racy = racy;
        }
    }

    public class Color {
        private double red;
        private double green;
        private double blue;
        private double alpha;

        public double Red { get => red; set => red = value; }
        public double Green { get => green; set => green = value; }
        public double Blue { get => blue; set => blue = value; }
        public double Alpha { get => alpha; set => alpha = value; }

        public Color(double red, double green, double blue, double alpha) {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }
    }

    public class ColorInfo {
        private Color color;
        private double score;
        private double pixelFraction;

        public Color Color { get => color; set => color = value; }
        public double Score { get => score; set => score = value; }
        public double PixelFraction { get => pixelFraction; set => pixelFraction = value; }

        public ColorInfo(Color color, double score, double pixelFraction) {
            this.Color = color;
            this.Score = score;
            this.PixelFraction = pixelFraction;
        }
    }

    public class DominantColorsAnnotation {
        private ColorInfo[] colors;

        public ColorInfo[] Colors { get => colors; set => colors = value; }

        public DominantColorsAnnotation(ColorInfo[] colors) {
            this.Colors = colors;
        }
    }

    public class ImagesProperties {
        private DominantColorsAnnotation dominantColors;

        public DominantColorsAnnotation DominantColors { get => dominantColors; set => dominantColors = value; }

        public DominantColorsAnnotation(DominantColorsAnnotation dominantColors) {
            this.DominantColors = dominantColors;
        }
    }

    public class CropHint {
        private BoundingPoly boundingPoly;
        private double confidence;
        private double importanceFraction;

        internal BoundingPoly BoundingPoly { get => boundingPoly; set => boundingPoly = value; }
        public double Confidence { get => confidence; set => confidence = value; }
        public double ImportanceFraction { get => importanceFraction; set => importanceFraction = value; }

        public CropHint(BoundingPoly boundingPoly, double confidence, double importanceFraction) {
            this.BoundingPoly = boundingPoly;
            this.Confidence = confidence;
            this.ImportanceFraction = importanceFraction;
        }
    }

    public class CropHintsAnnotation {
        private CropHint[] cropHints;

        public CropHint[] CropHints { get => cropHints; set => cropHints = value; }

        public CropHintsAnnotation(CropHint[] cropHints) {
            this.CropHints = cropHints;
        }
    }

    public class WebEntity {
        private String entity_id;
        private String description;
        private double relevancy_score;

        public String Entity_id { get => entity_id; set => entity_id = value; }
        public String Description { get => description; set => description = value; }
        public double Relevancy_score { get => relevancy_score; set => relevancy_score = value;}

        public WebEntity(String entity_id, String description, double relevancy_score) {
            this.Entity_id = entity_id;
            this.Description = description;
            this.Relevancy_score = relevancy_score;
        }
    }

    public class WebImage {
        private String url;
        private double relevancy_score;

        public String URL { get => url; set => url = value; }
        public double Relevancy_score { get => relevancy_score; set => relevancy_score = value;}

        public WebImage(String url, double relevancy_score) {
            this.URL = url;
            this.Relevancy_score = relevancy_score;
        }
    }

    public class WebPage {
        private String url;
        private double relevancy_score;
        private String pageTitle;
        private WebImage[] fullMatchingImages;
        private WebImage[] partialMatchingImages;

        public String URL { get => url; set => url = value; }
        public double Relevancy_score { get => relevancy_score; set => relevancy_score = value;}
        public String PageTitle { get => pageTitle; set => pageTitle = value; }
        internal WebImage[] FullMatchingImages { get => fullMatchingImages; set => fullMatchingImages = value; }
        internal WebImage[] PartialMatchingImages { get => partialMatchingImages; set => partialMatchingImages = value; }

        public WebImage(String url, double relevancy_score, String pageTitle, WebImage[] fullMatchingImages, WebImage[] partialMatchingImages) {
            this.URL = url;
            this.Relevancy_score = relevancy_score;
            this.PageTitle = pageTitle;
            this.FullMatchingImages = fullMatchingImages;
            this.PartialMatchingImages = partialMatchingImages;
        }
    }

    public class WebLabel {
        private String label;
        private String language_code;

        public String Label { get => label; set => label = value; }
        public String Language_code { get => language_code; set => language_code = value; }

        public WebLabel(String label, String language_code){
            this.Label = label;
            this.Language_code = language_code;
        }
    }
    
    public class WebDetection {
        private WebEntity[] webEntities;
        private WebImage[] fullMatchingImages;
        private WebImage[] partialMatchingImages;
        private WebPage[] pagesWithMatchingImages;
        private WebImage[] visuallySimilarImages;
        private WebLabel[] bestGuessLabels;

        internal WebEntity[] WebEntities { get => webEntities; set => webEntities = value; }
        internal WebImage[] FullMatchingImages { get => fullMatchingImages; set => fullMatchingImages = value; }
        internal WebImage[] PartialMatchingImages { get => partialMatchingImages; set => partialMatchingImages = value; }
        internal WebPage[] PagesWithMatchingImages { get => pagesWithMatchingImages; set => pagesWithMatchingImages = value; }
        internal WebImage[] VisuallySimilarImages { get => visuallySimilarImages; set => visuallySimilarImages = value; }
        internal WebLabel[] BestGuessLabels { get => bestGuessLabels; set => bestGuessLabels = value; }

        public WebDetection(WebEntity[] webEntities, WebImage[] fullMatchingImages, WebImage[] partialMatchingImages, WebPage[] pagesWithMatchingImages, WebImage[] visuallySimilarImages, WebLabel[] bestGuessLabels) {
            this.WebEntities = webEntities;
            this.FullMatchingImages = fullMatchingImages;
            this.PartialMatchingImages = partialMatchingImages;
            this.PagesWithMatchingImages = pagesWithMatchingImages;
            this.VisuallySimilarImages = visuallySimilarImages;
            this.BestGuessLabels = bestGuessLabels;
        }
    }

    public class AnnotateImageResponse {
        private FaceAnnotation faceAnnotations;
        private EntityAnnotation[] landmarkAnnotations;
        private EntityAnnotation[] logoAnnotations;
        private EntityAnnotation[] labelAnnotations;
        private EntityAnnotation[] textAnnotations;
        private TextAnnotation fullTextAnnotations;
        private SafeSearchAnnotation safeSearchAnnotations;
        private ImagesProperties imagesPropertiesAnnotations;
        private CropHintsAnnotation cropHintsAnnotations;
        private WebDetection webDetection;

        internal FaceAnnotation FaceAnnotations { get => faceAnnotations; set => faceAnnotations = value; }
        internal EntityAnnotation[] LandmarkAnnotations { get => landmarkAnnotations; set => landmarkAnnotations = value; }
        internal EntityAnnotation[] LogoAnnotations { get => logoAnnotations; set => logoAnnotations = value; }
        internal EntityAnnotation[] TextAnnotations { get => textAnnotations; set => textAnnotations = value; }
        internal TextAnnotation FullTextAnnotations { get => fullTextAnnotations; set => fullTextAnnotations = value; }
        internal SafeSearchAnnotation SafeSearchAnnotations { get => safeSearchAnnotations; set => safeSearchAnnotations = value; }
        internal ImagesProperties ImagePropertiesAnnotations { get => imagePropertiesAnnotations; set => imagePropertiesAnnotations = value; }
        internal CropHintsAnnotation CropHintsAnnotations { get => cropHintsAnnotations; set => cropHintsAnnotations = value; }
        internal WebDetection WebDetection { get => webDetection; set => webDetection = value; }

        public AnnotateImageResponse(FaceAnnotation faceAnnotation, EntityAnnotation[] landmarkAnnotations, EntityAnnotation[] logoAnnotations, EntityAnnotation[] textAnnotations, TextAnnotation fullTextAnnotations, SafeSearchAnnotation safeSearchAnnotations, ImagesProperties imagesPropertiesAnnotations, CropHintsAnnotation cropHintsAnnotations, WebDetection webDetection) {
            this.FaceAnnotations = faceAnnotation;
            this.LandmarkAnnotations - landmarkAnnotations;
            this.LogoAnnotations = logoAnnotations;
            this.TextAnnotations = textAnnotations;
            this.FullTextAnnotations = fullTextAnnotations;
            this.SafeSearchAnnotations = safeSearchAnnotations;
            this.ImagePropertiesAnnotations = imagesPropertiesAnnotations;
            this.CropHintsAnnotations = cropHintsAnnotations;
            this.WebDetection = webDetection;
        }
    }
}