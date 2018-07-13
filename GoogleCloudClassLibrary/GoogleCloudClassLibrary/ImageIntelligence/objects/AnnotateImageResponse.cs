using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
