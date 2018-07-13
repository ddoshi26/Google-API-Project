using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class ImageSource {
        private String imageUri;

        [JsonProperty("imageUri")]
        public String ImageUri {
            get => imageUri;
            set => imageUri = value;
        }

        public ImageSource(String imageUri) {
            this.ImageUri = imageUri;
        }
    }
}
