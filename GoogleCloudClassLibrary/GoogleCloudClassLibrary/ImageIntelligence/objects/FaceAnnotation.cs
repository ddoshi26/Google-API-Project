using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
