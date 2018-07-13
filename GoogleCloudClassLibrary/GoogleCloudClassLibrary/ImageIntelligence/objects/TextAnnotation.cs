using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class TextAnnotation {
        private List<Page> pages;
        private String text;

        [JsonProperty("pages")]
        public List<Page> Pages {
            get => pages; set => pages = value;
        }

        [JsonProperty("text")]
        public String Text {
            get => text; set => text = value;
        }
    
        public TextAnnotation(List<Page> pages, String text) {
            this.Pages = pages;
            this.Text = text;
        }
    }
}
