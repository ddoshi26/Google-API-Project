using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary {
    public class ErrorLink {
        private String description;
        private String url;

        [JsonProperty("description")]
        public string Description {
            get => description;
            set => description = value;
        }

        [JsonProperty("url")]
        public string Url {
            get => url;
            set => url = value;
        }

        public ErrorLink(string description, string url) {
            Description = description;
            Url = url;
        }
    }
}
