using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
