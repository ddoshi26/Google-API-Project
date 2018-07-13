using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.Places {
    public class Id {
        private String place_id;
        private String scope;

        [JsonProperty("place_id")]
        public string Place_id {
            get => place_id; set => place_id = value;
        }

        [JsonProperty("scope")]
        public string Scope {
            get => scope; set => scope = value;
        }

        public Id() {
            this.Place_id = "";
            this.Scope = "";
        }

        public Id(String place_id, String scope) {
            this.Place_id = place_id;
            this.Scope = scope;
        }
    }
}
