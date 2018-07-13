using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class Property {
        private String name;
        private String val;
        private String uint64Value;

        [JsonProperty("name")]
        public String Name {
            get => name; set => name = value;
        }

        [JsonProperty("value")]
        public String Value {
            get => val; set => val = value;
        }

        [JsonProperty("uint64Value")]
        public String Uint64Value {
            get => uint64Value; set => uint64Value = value;
        }

        public Property(String name, String value, String uint64Value) {
            this.Name = name;
            this.Value = value;
            this.Uint64Value = uint64Value;
        }
    }
}
