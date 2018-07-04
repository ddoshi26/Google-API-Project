using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class ImageSource {
        private String imageUri;

        [JsonProperty("imageUri")]
        public String ImageUri {
            get => imageUri;
            set => imageUri = value;
        }

        public ImageSource(String imageUri) {
            this.ImageUri = imageUri;
        }
    }

    public class Image {
        private String content;
        private ImageSource source;

        [JsonProperty("content")]
        public String Content {
            get => content; set => content = value;
        }

        [JsonProperty("source")]
        public ImageSource Source {
            get => source; set => source = value;
        }

        public Image(String content = "", ImageSource source = null) {
            this.Content = content;
            this.Source = source;
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

        [JsonProperty("type")]
        public ImageType Type {
            get => type; set => type = value;
        }

        [JsonProperty("maxResults")]
        public int MaxResults {
            get => maxResults; set => maxResults = value;
        }

        [JsonProperty("model")]
        public String Model {
            get => model; set => model = value;
        }

        public ImageFeatures(ImageType type, int maxResults, String model) {
            this.Type = type;
            this.MaxResults = maxResults;
            this.Model = model;
        }
    }

    public class LatLng {
        private Double latitude;
        private Double longitude;

        [JsonProperty("latitude")]
        public double Latitude {
            get => latitude; set => latitude = value;
        }

        [JsonProperty("longitude")]
        public double Longitude {
            get => longitude; set => longitude = value;
        }

        public LatLng(Double lat, Double lng) {
            this.Latitude = lat;
            this.Longitude = lng;
        }
    }

    public class LatLongRect {
        private LatLng minLatLng;
        private LatLng maxLatLng;

        [JsonProperty("minLatLng")]
        public LatLng MinLatLng {
            get => minLatLng; set => minLatLng = value;
        }

        [JsonProperty("maxLatLng")]
        public LatLng MaxLatLng {
            get => maxLatLng; set => maxLatLng = value;
        }

        public LatLongRect(LatLng minLatLng, LatLng maxLatLng) {
            this.MinLatLng = minLatLng;
            this.MaxLatLng = maxLatLng;
        }
    }

    public class CropHintsParams {
        private List<double> aspectRatios;

        [JsonProperty("aspectRatios")]
        public List<double> AspectRatios {
            get => aspectRatios;
            set => aspectRatios = value;
        }

        public CropHintsParams(List<double> aspectRatios) {
            AspectRatios = aspectRatios;
        }
    }

    public class WebDetectionParams {
        private Boolean includeGeoResults;

        [JsonProperty("includeGeoResults")]
        public bool IncludeGeoResults {
            get => includeGeoResults;
            set => includeGeoResults = value;
        }

        public WebDetectionParams(bool includeGeoResults) {
            IncludeGeoResults = includeGeoResults;
        }
    }

    public class ImageContext {
        private LatLongRect latLongRect;
        private List<String> languageHints;
        private CropHintsParams cropHintParams;
        private WebDetectionParams webDetectionParams;

        [JsonProperty("latLongRect")]
        public LatLongRect LatLongRect {
            get => latLongRect; set => latLongRect = value;
        }

        [JsonProperty("languageHints")]
        public List<String> LanguageHints {
            get => languageHints; set => languageHints = value;
        }

        [JsonProperty("cropHintsParams")]
        public CropHintsParams CropHintsParams {
            get => cropHintParams; set => cropHintParams = value;
        }

        [JsonProperty("webDetectionParams")]
        public WebDetectionParams WebDetectionParams {
            get => webDetectionParams; set => webDetectionParams = value;
        }

        public ImageContext(LatLongRect latLngRect, List<String> langHints, CropHintsParams cropHintsParams, WebDetectionParams webDetectionParams) {
            this.LatLongRect = latLngRect;
            this.LanguageHints = langHints;
            this.CropHintsParams = cropHintsParams;
            this.WebDetectionParams = webDetectionParams;
        }
    }

    // TODO: Continue from here

    public class AnnotateImageRequests {
        private Image image;
        private List<ImageFeatures> features;
        private ImageContext imageContext;

        [JsonProperty("image")]
        public Image Image {
            get => image; set => image = value;
        }

        [JsonProperty("features")]
        public List<ImageFeatures> Features {
            get => features; set => features = value;
        }

        [JsonProperty("imageContext")]
        public ImageContext ImageContext {
            get => imageContext; set => imageContext = value;
        }

        public AnnotateImageRequests(Image image, List<ImageFeatures> features, ImageContext context) {
            this.Image = image;
            this.Features = features;
            this.ImageContext = context;
        }
    }

    public class AnnotateImageRequestList {
        private List<AnnotateImageRequests> requests;

        [JsonProperty("requests")]
        public List<AnnotateImageRequests> Requests {
            get => requests; set => requests = value;
        }

        public AnnotateImageRequestList(List<AnnotateImageRequests> requests) {
            this.Requests = requests;
        }
    }

    public class Vertex {
        private double x;
        private double y;

        [JsonProperty("x")]
        public double X {
            get => x; set => x = value;
        }

        [JsonProperty("y")]
        public double Y {
            get => y; set => y = value;
        }

        public Vertex(double x, double y) {
            this.X = x;
            this.Y = y;
        }
    }

    public class BoundingPoly {
        private List<Vertex> vertices;

        [JsonProperty("vertices")]
        public List<Vertex> Vertices {
            get => vertices; set => vertices = value;
        }

        public BoundingPoly(List<Vertex> vertices) {
            this.Vertices = vertices;
        }
    }

    public enum LandmarkType {
        UNKNOWN_LANDMARK, LEFT_EYE, RIGHT_EYE, LEFT_OF_LEFT_EYEBROW, RIGHT_OF_LEFT_EYEBROW, LEFT_OF_RIGHT_EYEBROW, RIGHT_OF_RIGHT_EYEBROW, MIDPOINT_BETWEEN_EYES, NOSE_TIP, UPPER_LIP, LOWER_LIP, MOUTH_LEFT, MOUTH_RIGHT, MOUTH_CENTER, NOSE_BOTTOM_RIGHT, NOSE_BOTTOM_LEFT, NOSE_BOTTOM_CENTER, LEFT_EYE_TOP_BOUNDARY, LEFT_EYE_BOTTOM_BOUNDARY, LEFT_EYE_RIGHT_CORNER, LEFT_EYE_LEFT_CORNER, RIGHT_EYE_TOP_BOUNDARY, RIGHT_EYE_RIGHT_CORNER, RIGHT_EYE_BOTTOM_BOUNDARY, RIGHT_EYE_LEFT_CORNER, LEFT_EYEBROW_UPPER_MIDPOINT, RIGHT_EYEBROW_UPPER_MIDPOINT, LEFT_EAR_TRAGION, RIGHT_EAR_TRAGION, LEFT_EYE_PUPIL, RIGHT_EYE_PUPIL, FOREHEAD_GLABELLA, CHIN_GNATHION, CHIN_LEFT_GONION, CHIN_RIGHT_GONION
    }

    public class Position {
        double x;
        double y;
        double z;

        [JsonProperty("x")]
        public double X {
            get => x; set => x = value;
        }

        [JsonProperty("y")]
        public double Y {
            get => y; set => y = value;
        }

        [JsonProperty("z")]
        public double Z {
            get => z; set => z = value;
        }

        public Position(double x, double y, double z) {
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

        [JsonProperty("type")]
        public LandmarkType Type {
            get => type; set => type = value;
        }

        [JsonProperty("position")]
        public Position Position {
            get => position; set => position = value;
        }

        public Landmark(LandmarkType type, Position position) {
            this.Type = type;
            this.Position = position;
        }
    }

    public class FaceAnnotation {
        private BoundingPoly boundingPoly;
        private BoundingPoly fdBoundingPoly;
        private List<Landmark> landmarks;
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

        [JsonProperty("boundingPoly")]
        public BoundingPoly BoundingPoly {
            get => boundingPoly; set => boundingPoly = value;
        }

        [JsonProperty("fdBoundingPoly")]
        public BoundingPoly FDBoundingPoly {
            get => fdBoundingPoly; set => fdBoundingPoly = value;
        }

        [JsonProperty("landmarks")]
        public List<Landmark> Landmarks {
            get => landmarks; set => landmarks = value;
        }

        [JsonProperty("rollAngle")]
        public double RollAngle {
            get => rollAngle; set => rollAngle = value;
        }

        [JsonProperty("panAngle")]
        public double PanAngle {
            get => panAngle; set => panAngle = value;
        }

        [JsonProperty("tiltAngle")]
        public double TiltAngle {
            get => tiltAngle; set => tiltAngle = value;
        }

        [JsonProperty("detectionConfidence")]
        public double DetectionConfidence {
            get => detectionConfidence; set => detectionConfidence = value;
        }

        [JsonProperty("landmarkingConfidence")]
        public double LandmarkingConfidence {
            get => landmarkingConfidence; set => landmarkingConfidence = value;
        }

        [JsonProperty("joyLikelihood")]
        public Likelihood JoyLikelihood {
            get => joyLikelihood; set => joyLikelihood = value;
        }

        [JsonProperty("sorrowLikelihood")]
        public Likelihood SorrowLikelihood {
            get => sorrowLikelihood; set => sorrowLikelihood = value;
        }

        [JsonProperty("angerLikelihood")]
        public Likelihood AngerLikelihood {
            get => angerLikelihood; set => angerLikelihood = value;
        }

        [JsonProperty("surpriseLikelihood")]
        public Likelihood SurpriseLikelihood {
            get => surpriseLikelihood; set => surpriseLikelihood = value;
        }

        [JsonProperty("underExposedLikelihood")]
        public Likelihood UnderExposedLikelihood {
            get => underExposedLikelihood; set => underExposedLikelihood = value;
        }

        [JsonProperty("blurredLikelihood")]
        public Likelihood BlurredLikelihood {
            get => blurredLikelihood; set => blurredLikelihood = value;
        }

        [JsonProperty("headwearLikelihood")]
        public Likelihood HeadwearLikelihood {
            get => headwearLikelihood; set => headwearLikelihood = value;
        }

        public FaceAnnotation(BoundingPoly boundingPoly, BoundingPoly fdBoundingPoly, List<Landmark> landmarks, double rollAngle, double panAngle, double tiltAngle, double detectionConfidence, double landmarkingConfidence, Likelihood joyLikelihood, Likelihood sorrowLikelihood, Likelihood angerLikelihood, Likelihood surpriseLikelihood, Likelihood underExposedLikelihood, Likelihood blurredLikelihood, Likelihood headwearLikelihood) {
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

    // TODO: Check
    public class LocationInfo {
        private LatLng latLng;

        [JsonProperty("latLng")]
        public LatLng LatLng {
            get => latLng; set => latLng = value;
        }

        public LocationInfo(LatLng latLng) {
            this.LatLng = latLng;
        }
    }

    public class Property {
        private String name;
        private String val;
        private String uint64Value;

        [JsonProperty("name")]
        public String Name {
            get => name; set => name = value;
        }

        [JsonProperty("value")]
        public String Value {
            get => val; set => val = value;
        }

        [JsonProperty("uint64Value")]
        public String Uint64Value {
            get => uint64Value; set => uint64Value = value;
        }

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
        private List<LocationInfo> locations;
        private List<Property> properties;

        [JsonProperty("mid")]
        public String Mid {
            get => mid; set => mid = value;
        }

        [JsonProperty("locale")]
        public String Locale {
            get => locale; set => locale = value;
        }

        [JsonProperty("description")]
        public String Description {
            get => description; set => description = value;
        }

        [JsonProperty("score")]
        public double Score {
            get => score; set => score = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        [JsonProperty("topicality")]
        public double Topicality {
            get => topicality; set => topicality = value;
        }

        [JsonProperty("boundingPoly")]
        public BoundingPoly BoundingPoly {
            get => boundingPoly; set => boundingPoly = value;
        }

        [JsonProperty("locations")]
        public List<LocationInfo> Locations {
            get => locations; set => locations = value;
        }

        [JsonProperty("properties")]
        public List<Property> Properties {
            get => properties; set => properties = value;
        }

        public EntityAnnotation(String mid, String locale, String description, double score, double confidence,
            double topicality, BoundingPoly boundingPoly, List<LocationInfo> locations, List<Property> properties) {
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

        [JsonProperty("languageCode")]
        public String LanguageCode {
            get => languageCode; set => languageCode = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

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

        [JsonProperty("type")]
        public BreakType Type {
            get => type; set => type = value;
        }

        [JsonProperty("isPrefix")]
        public Boolean IsPrefix {
            get => isPrefix; set => isPrefix = value;
        }

        public DetectedBreak(BreakType type, Boolean isPrefix) {
            this.Type = type;
            this.IsPrefix = isPrefix;
        }
    }

    public class TextProperty {
        private List<DetectedLanguage> detectedLanguages;
        private DetectedBreak detectedBreak;

        [JsonProperty("detectedLanguages")]
        public List<DetectedLanguage> DetectedLanguages {
            get => detectedLanguages; set => detectedLanguages = value;
        }

        [JsonProperty("detectedBreak")]
        public DetectedBreak DetectedBreak {
            get => detectedBreak; set => detectedBreak = value;
        }

        public TextProperty(List<DetectedLanguage> detectedLanguages, DetectedBreak detectedBreak) {
            this.DetectedLanguages = detectedLanguages;
            this.DetectedBreak = detectedBreak;
        }
    }

    public class Symbol {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private String text;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("boundingBox")]
        public BoundingPoly BoundingBox {
            get => boundingBox; set => boundingBox = value;
        }

        [JsonProperty("text")]
        public String Text {
            get => text; set => text = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

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
        private List<Symbol> symbols;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("boundingBox")]
        public BoundingPoly BoundingBox {
            get => boundingBox; set => boundingBox = value;
        }

        [JsonProperty("symbols")]
        public List<Symbol> Symbols {
            get => symbols; set => symbols = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Word(TextProperty property, BoundingPoly boundingBox, List<Symbol> symbols, double confidence) {
            this.Property = property;
            this.BoundingBox = boundingBox;
            this.Symbols = symbols;
            this.Confidence = confidence;
        }
    }

    public class Paragraph {
        private TextProperty property;
        private BoundingPoly boundingBox;
        private List<Word> words;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("boundingBox")]
        public BoundingPoly BoundingBox {
            get => boundingBox; set => boundingBox = value;
        }

        [JsonProperty("words")]
        public List<Word> Words {
            get => words; set => words = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Paragraph(TextProperty property, BoundingPoly boundingBox, List<Word> words, double confidence) {
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
        private List<Paragraph> paragraphs;
        private BlockType blockType;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("boundingBox")]
        public BoundingPoly BoundingBox {
            get => boundingBox; set => boundingBox = value;
        }

        [JsonProperty("paragraphs")]
        public List<Paragraph> Paragraphs {
            get => paragraphs; set => paragraphs = value;
        }

        [JsonProperty("blockType")]
        public BlockType BlockType {
            get => blockType; set => blockType = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Block(TextProperty property, BoundingPoly boundingBox, List<Paragraph> paragraphs, BlockType blockType, double confidence) {
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
        private List<Block> blocks;
        private double confidence;

        [JsonProperty("property")]
        public TextProperty Property {
            get => property; set => property = value;
        }

        [JsonProperty("width")]
        public double Width {
            get => width; set => width = value;
        }

        [JsonProperty("height")]
        public double Height {
            get => height; set => height = value;
        }

        [JsonProperty("blocks")]
        public List<Block> Blocks {
            get => blocks; set => blocks = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        public Page(TextProperty property, double width, double height, List<Block> blocks, double confidence) {
            this.Property = property;
            this.Width = width;
            this.Height = height;
            this.Blocks = blocks;
            this.Confidence = confidence;
        }
    }

    public class TextAnnotation {
        private List<Page> pages;
        private String text;

        [JsonProperty("pages")]
        public List<Page> Pages {
            get => pages; set => pages = value;
        }

        [JsonProperty("text")]
        public String Text {
            get => text; set => text = value;
        }
    
        public TextAnnotation(List<Page> pages, String text) {
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

        [JsonProperty("adult")]
        public Likelihood Adult {
            get => adult; set => adult = value;
        }

        [JsonProperty("spoof")]
        public Likelihood Spoof {
            get => spoof; set => spoof = value;
        }

        [JsonProperty("medical")]
        public Likelihood Medical {
            get => medical; set => medical = value;
        }

        [JsonProperty("violence")]
        public Likelihood Violence {
            get => violence; set => violence = value;
        }

        [JsonProperty("racy")]
        public Likelihood Racy {
            get => racy; set => racy = value;
        }

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

        [JsonProperty("red")]
        public double Red {
            get => red; set => red = value;
        }

        [JsonProperty("green")]
        public double Green {
            get => green; set => green = value;
        }

        [JsonProperty("blue")]
        public double Blue {
            get => blue; set => blue = value;
        }

        [JsonProperty("alpha")]
        public double Alpha {
            get => alpha; set => alpha = value;
        }

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

        [JsonProperty("color")]
        public Color Color {
            get => color; set => color = value;
        }

        [JsonProperty("score")]
        public double Score {
            get => score; set => score = value;
        }

        [JsonProperty("pixelFraction")]
        public double PixelFraction {
            get => pixelFraction; set => pixelFraction = value;
        }

        public ColorInfo(Color color, double score, double pixelFraction) {
            this.Color = color;
            this.Score = score;
            this.PixelFraction = pixelFraction;
        }
    }

    public class DominantColorsAnnotation {
        private List<ColorInfo> colors;

        [JsonProperty("colors")]
        public List<ColorInfo> Colors {
            get => colors; set => colors = value;
        }

        public DominantColorsAnnotation(List<ColorInfo> colors) {
            this.Colors = colors;
        }
    }

    public class ImagesProperties {
        private DominantColorsAnnotation dominantColors;

        [JsonProperty("dominantColors")]
        public DominantColorsAnnotation DominantColors {
            get => dominantColors; set => dominantColors = value;
        }

        public ImagesProperties(DominantColorsAnnotation dominantColors) {
            this.DominantColors = dominantColors;
        }
    }

    public class CropHint {
        private BoundingPoly boundingPoly;
        private double confidence;
        private double importanceFraction;

        [JsonProperty("boundingPoly")]
        public BoundingPoly BoundingPoly {
            get => boundingPoly; set => boundingPoly = value;
        }

        [JsonProperty("confidence")]
        public double Confidence {
            get => confidence; set => confidence = value;
        }

        [JsonProperty("importanceFraction")]
        public double ImportanceFraction {
            get => importanceFraction; set => importanceFraction = value;
        }

        public CropHint(BoundingPoly boundingPoly, double confidence, double importanceFraction) {
            this.BoundingPoly = boundingPoly;
            this.Confidence = confidence;
            this.ImportanceFraction = importanceFraction;
        }
    }

    public class CropHintsAnnotation {
        private List<CropHint> cropHints;

        [JsonProperty("cropHints")]
        public List<CropHint> CropHints {
            get => cropHints; set => cropHints = value;
        }

        public CropHintsAnnotation(List<CropHint> cropHints) {
            this.CropHints = cropHints;
        }
    }

    public class WebEntity {
        private String entityid;
        private String description;
        private double relevancyscore;

        [JsonProperty("entityId")]
        public String EntityId {
            get => entityid; set => entityid = value;
        }

        [JsonProperty("description")]
        public String Description {
            get => description; set => description = value;
        }

        [JsonProperty("score")]
        public double Relevancyscore {
            get => relevancyscore; set => relevancyscore = value;
        }

        public WebEntity(String entityId, String description, double score) {
            this.EntityId = entityId;
            this.Description = description;
            this.Relevancyscore = score;
        }
    }

    public class WebImage {
        private String url;
        private double score;

        [JsonProperty("url")]
        public String URL {
            get => url; set => url = value;
        }

        [JsonProperty("score")]
        public double Score {
            get => score; set => score = value;
        }

        public WebImage(String url, double score) {
            this.URL = url;
            this.Score = score;
        }
    }

    public class WebPage {
        private String url;
        private double relevancy_score;
        private String pageTitle;
        private List<WebImage> fullMatchingImages;
        private List<WebImage> partialMatchingImages;

        [JsonProperty("url")]
        public String URL {
            get => url; set => url = value;
        }

        [JsonProperty("score")]
        public double Relevancy_score {
            get => relevancy_score; set => relevancy_score = value;
        }

        [JsonProperty("pageTitle")]
        public String PageTitle {
            get => pageTitle; set => pageTitle = value;
        }

        [JsonProperty("fullMatchingImages")]
        public List<WebImage> FullMatchingImages {
            get => fullMatchingImages; set => fullMatchingImages = value;
        }

        [JsonProperty("partialMatchingImages")]
        public List<WebImage> PartialMatchingImages {
            get => partialMatchingImages; set => partialMatchingImages = value;
        }

        public WebPage(String url, double relevancy_score, String pageTitle, List<WebImage> fullMatchingImages, List<WebImage> partialMatchingImages) {
            this.URL = url;
            this.Relevancy_score = relevancy_score;
            this.PageTitle = pageTitle;
            this.FullMatchingImages = fullMatchingImages;
            this.PartialMatchingImages = partialMatchingImages;
        }
    }

    public class WebLabel {
        private String label;
        private String languageCode;

        [JsonProperty("label")]
        public String Label {
            get => label; set => label = value;
        }

        [JsonProperty("languageCode")]
        public String LanguageCode {
            get => languageCode; set => languageCode = value;
        }

        public WebLabel(String label, String languageCode) {
            this.Label = label;
            this.LanguageCode = languageCode;
        }
    }

    public class WebDetection {
        private List<WebEntity> webEntities;
        private List<WebImage> fullMatchingImages;
        private List<WebImage> partialMatchingImages;
        private List<WebPage> pagesWithMatchingImages;
        private List<WebImage> visuallySimilarImages;
        private List<WebLabel> bestGuessLabels;

        [JsonProperty("webEntities")]
        public List<WebEntity> WebEntities {
            get => webEntities; set => webEntities = value;
        }

        [JsonProperty("fullMatchingImages")]
        public List<WebImage> FullMatchingImages {
            get => fullMatchingImages; set => fullMatchingImages = value;
        }

        [JsonProperty("partialMatchingImages")]
        public List<WebImage> PartialMatchingImages {
            get => partialMatchingImages; set => partialMatchingImages = value;
        }

        [JsonProperty("pagesWithMatchingImages")]
        public List<WebPage> PagesWithMatchingImages {
            get => pagesWithMatchingImages; set => pagesWithMatchingImages = value;
        }

        [JsonProperty("visuallySimilarImages")]
        public List<WebImage> VisuallySimilarImages {
            get => visuallySimilarImages; set => visuallySimilarImages = value;
        }

        [JsonProperty("bestGuessLabels")]
        public List<WebLabel> BestGuessLabels {
            get => bestGuessLabels; set => bestGuessLabels = value;
        }

        public WebDetection(List<WebEntity> webEntities, List<WebImage> fullMatchingImages, List<WebImage> partialMatchingImages, List<WebPage> pagesWithMatchingImages, List<WebImage> visuallySimilarImages, List<WebLabel> bestGuessLabels) {
            this.WebEntities = webEntities;
            this.FullMatchingImages = fullMatchingImages;
            this.PartialMatchingImages = partialMatchingImages;
            this.PagesWithMatchingImages = pagesWithMatchingImages;
            this.VisuallySimilarImages = visuallySimilarImages;
            this.BestGuessLabels = bestGuessLabels;
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

    public class AnnotateImageResponse {
        private List<FaceAnnotation> faceAnnotations;
        private List<EntityAnnotation> landmarkAnnotations;
        private List<EntityAnnotation> logoAnnotations;
        private List<EntityAnnotation> labelAnnotations;
        private List<EntityAnnotation> textAnnotations;
        private TextAnnotation fullTextAnnotations;
        private SafeSearchAnnotation safeSearchAnnotations;
        private ImagesProperties imagePropertiesAnnotation;
        private CropHintsAnnotation cropHintsAnnotation;
        private WebDetection webDetection;
        private Status error;

        [JsonProperty("faceAnnotations")]
        public List<FaceAnnotation> FaceAnnotations {
            get => faceAnnotations; set => faceAnnotations = value;
        }

        [JsonProperty("landmarkAnnotations")]
        public List<EntityAnnotation> LandmarkAnnotations {
            get => landmarkAnnotations; set => landmarkAnnotations = value;
        }

        [JsonProperty("logoAnnotations")]
        public List<EntityAnnotation> LogoAnnotations {
            get => logoAnnotations; set => logoAnnotations = value;
        }

        [JsonProperty("labelAnnotations")]
        public List<EntityAnnotation> LabelAnnotations {
            get => labelAnnotations; set => labelAnnotations = value;
        }

        [JsonProperty("textAnnotations")]
        public List<EntityAnnotation> TextAnnotations {
            get => textAnnotations; set => textAnnotations = value;
        }

        [JsonProperty("fullTextAnnotation")]
        public TextAnnotation FullTextAnnotations {
            get => fullTextAnnotations; set => fullTextAnnotations = value;
        }

        [JsonProperty("safeSearchAnnotation")]
        public SafeSearchAnnotation SafeSearchAnnotations {
            get => safeSearchAnnotations; set => safeSearchAnnotations = value;
        }

        [JsonProperty("imagePropertiesAnnotation")]
        public ImagesProperties ImagePropertiesAnnotation {
            get => imagePropertiesAnnotation; set => imagePropertiesAnnotation = value;
        }

        [JsonProperty("cropHintsAnnotation")]
        public CropHintsAnnotation CropHintsAnnotation {
            get => cropHintsAnnotation; set => cropHintsAnnotation = value;
        }

        [JsonProperty("webDetection")]
        public WebDetection WebDetection {
            get => webDetection; set => webDetection = value;
        }

        [JsonProperty("error")]
        public Status Error {
            get => error; set => error = value;
        }

        public AnnotateImageResponse(List<FaceAnnotation> faceAnnotation, List<EntityAnnotation> landmarkAnnotations, List<EntityAnnotation> logoAnnotations, List<EntityAnnotation> labelAnnotations, List<EntityAnnotation> textAnnotations, TextAnnotation fullTextAnnotations, SafeSearchAnnotation safeSearchAnnotations, ImagesProperties imagePropertiesAnnotation, CropHintsAnnotation cropHintsAnnotation, WebDetection webDetection) {
            this.FaceAnnotations = faceAnnotation;
            this.LandmarkAnnotations = landmarkAnnotations;
            this.LogoAnnotations = logoAnnotations;
            this.LabelAnnotations = labelAnnotations;
            this.TextAnnotations = textAnnotations;
            this.FullTextAnnotations = fullTextAnnotations;
            this.SafeSearchAnnotations = safeSearchAnnotations;
            this.ImagePropertiesAnnotation = imagePropertiesAnnotation;
            this.CropHintsAnnotation = cropHintsAnnotation;
            this.WebDetection = webDetection;
        }
    }

    public class AnnotateImageResponseList {
        private List<AnnotateImageResponse> responses;

        [JsonProperty("responses")]
        public List<AnnotateImageResponse> Responses { get => responses; set => responses = value; }

        public AnnotateImageResponseList(List<AnnotateImageResponse> responses) {
            Responses = responses;
        }
    }
}
