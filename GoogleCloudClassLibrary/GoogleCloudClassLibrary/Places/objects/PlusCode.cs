using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.Places {
    public class PlusCode {
        private String compoundCode;
        private String globalCode;

        [JsonProperty("compound_code")]
        public string CompoundCode {
            get => compoundCode;
            set => compoundCode = value;
        }

        [JsonProperty("global_code")]
        public string GlobalCode {
            get => globalCode;
            set => globalCode = value;
        }

        public PlusCode(string compoundCode, string globalCode) {
            CompoundCode = compoundCode;
            GlobalCode = globalCode;
        }

    }
}
