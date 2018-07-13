using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
