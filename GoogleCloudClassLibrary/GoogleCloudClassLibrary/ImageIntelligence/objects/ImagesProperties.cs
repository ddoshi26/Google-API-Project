using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
