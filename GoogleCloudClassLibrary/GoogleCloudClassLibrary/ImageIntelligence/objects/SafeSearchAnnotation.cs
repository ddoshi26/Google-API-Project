using Newtonsoft.Json;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class SafeSearchAnnotation {
        private Likelihood adult;
        private Likelihood spoof;
        private Likelihood medical;
        private Likelihood violence;
        private Likelihood racy;

        [JsonProperty("adult")]
        public Likelihood Adult {
            get => adult; set => adult = value;
        }

        [JsonProperty("spoof")]
        public Likelihood Spoof {
            get => spoof; set => spoof = value;
        }

        [JsonProperty("medical")]
        public Likelihood Medical {
            get => medical; set => medical = value;
        }

        [JsonProperty("violence")]
        public Likelihood Violence {
            get => violence; set => violence = value;
        }

        [JsonProperty("racy")]
        public Likelihood Racy {
            get => racy; set => racy = value;
        }

        public SafeSearchAnnotation(Likelihood adult, Likelihood spoof, Likelihood medical, Likelihood violence, Likelihood racy) {
            this.Adult = adult;
            this.Spoof = spoof;
            this.Medical = medical;
            this.Violence = violence;
            this.Racy = racy;
        }
    }
}
