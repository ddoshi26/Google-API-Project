using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
