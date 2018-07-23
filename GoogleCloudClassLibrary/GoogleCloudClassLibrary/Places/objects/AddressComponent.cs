using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoogleCloudClassLibrary.Places {
    public class AddressComponent {
        private String longName;
        private String shortName;
        private List<String> types;

        [JsonProperty("long_name")]
        public string LongName {
            get => longName;
            set => longName = value;
        }

        [JsonProperty("short_name")]
        public string ShortName {
            get => shortName;
            set => shortName = value;
        }

        [JsonProperty("types")]
        public List<string> Types {
            get => types;
            set => types = value;
        }

        public AddressComponent(string longName, string shortName, List<string> types) {
            LongName = longName;
            ShortName = shortName;
            Types = types;
        }
    }
}
