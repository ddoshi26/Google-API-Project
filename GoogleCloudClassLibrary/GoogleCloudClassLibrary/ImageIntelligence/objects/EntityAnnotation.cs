using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
