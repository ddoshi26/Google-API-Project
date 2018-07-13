using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class TextSpan {
        String content;
        int beginOffset;

        [JsonProperty("content")]
        public String Content {
            get => content; set => content = value;
        }

        [JsonProperty("beginOffset")]
        public int BeginOffset {
            get => beginOffset; set => beginOffset = value;
        }

        public TextSpan(String content, int beginOffset) {
            this.Content = content;
            this.BeginOffset = beginOffset;
        }
    }
}
