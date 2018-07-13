using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
