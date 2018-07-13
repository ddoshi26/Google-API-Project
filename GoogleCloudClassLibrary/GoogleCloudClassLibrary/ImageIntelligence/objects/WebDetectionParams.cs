using Newtonsoft.Json;
using System;

namespace GoogleCloudClassLibrary.ImageIntelligence {
    public class WebDetectionParams {
        private Boolean includeGeoResults;

        [JsonProperty("includeGeoResults")]
        public bool IncludeGeoResults {
            get => includeGeoResults;
            set => includeGeoResults = value;
        }

        public WebDetectionParams(bool includeGeoResults) {
            IncludeGeoResults = includeGeoResults;
        }
    }
}
