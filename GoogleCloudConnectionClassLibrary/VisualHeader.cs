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

    // TODO: Complete this class 

    public class Operation {
        String name;
        Object metadata;
        Boolean operationDone;
        Object response;
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
}