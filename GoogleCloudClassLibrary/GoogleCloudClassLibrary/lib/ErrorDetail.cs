using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary {
    public class ErrorDetail {
        private String type;
        private List<ErrorLink> links;

        [JsonProperty("type")]
        public string Type {
            get => type;
            set => type = value;
        }

        [JsonProperty("links")]
        public List<ErrorLink> Links {
            get => links;
            set => links = value;
        }

        public ErrorDetail(string type, List<ErrorLink> links) {
            Type = type;
            Links = links;
        }
    }
}
