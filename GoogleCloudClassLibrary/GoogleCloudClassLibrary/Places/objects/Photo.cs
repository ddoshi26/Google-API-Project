using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class Photo {
        private Double height;
        private Double width;
        private String photo_ref;
        private List<String> html_attributions;

        [JsonProperty("height")]
        public double Height {
            get => height; private set => height = value;
        }

        [JsonProperty("width")]
        public double Width {
            get => width; private set => width = value;
        }

        [JsonProperty("photo_reference")]
        public string Photo_ref {
            get => photo_ref; private set => photo_ref = value;
        }

        [JsonProperty("html_attributions")]
        public List<string> Html_attributions {
            get => html_attributions; private set => html_attributions = value;
        }

        public Photo() {
            this.Height = 0.00;
            this.Width = 0.00;
            this.Photo_ref = "";
            this.Html_attributions = null;
        }

        public Photo(Double height, Double width, String photo_ref, List<String> html_atts) {
            this.Height = height;
            this.Width = width;
            this.Photo_ref = photo_ref;
            this.Html_attributions = html_atts;
        }
    }
}
