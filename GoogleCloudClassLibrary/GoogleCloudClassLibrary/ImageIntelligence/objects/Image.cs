using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
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
}
