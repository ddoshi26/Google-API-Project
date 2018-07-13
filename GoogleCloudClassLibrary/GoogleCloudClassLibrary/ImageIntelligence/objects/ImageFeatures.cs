﻿using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
